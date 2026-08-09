using FabulaUltimaGMTool;
using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Campaign : Container
{
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
    public CampaignData CampaignData { get; set; }

    [Export]
    public Configuration Configuration { get; set; }

    public UserConfigurationData _userConfiguration;

    [Export]
    public double SaveTimeWindowSeconds { get; set; } = 2;

    [Export]
    public Label SaveStatusLabel { get; set; }

    [Signal]
    public delegate void UpdateCurrentCampaignEventHandler(SignalWrapper<CampaignData> campaign);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        // fall back to resolving by path if the exported reference didn't populate
        SaveStatusLabel ??= GetNodeOrNull<Label>("CampaignName/SaveStatusLabel");

        if(Configuration != null)
        {
            Configuration.MakeCampaignDirectories();
        }

        _userConfiguration = GetNode<UserConfigurationState>("/root/UserConfigurationState").UserConfigurationData;

        // overwrite default if available
        if (!string.IsNullOrEmpty(_userConfiguration.CurrentCampaignID))
        {
            var filePath = GetCampaignFilePath(_userConfiguration.CurrentCampaignID);
            var stored = ResourceExtensions.Load<CampaignData>(filePath);
            if (stored != null)
            {
                CampaignData = stored;
            }
            else
            {
                // keep the exported default campaign rather than running with none:
                // a null here used to abort _Ready, leaving the app half-wired
                // (no save subscriber, NREs on Add Scene) and silently losing edits
                GD.PushError($"couldn't load campaign {filePath}; falling back to the default campaign");
                ShowLoadFailure($"Couldn't read the saved campaign:\n{filePath}\n\nLoaded the default campaign instead. The saved file was not overwritten.");
            }
        }

        if (CampaignData != null)
        {
            UpdateCampaign(CampaignData, true);
        }

        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<SaveMessage>(this.ReceiveSaveMessage);
        messageRouter.RegisterSubscriber<CampaignUpdate>(this.ReceiveCampaignUpdate);
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();        
    }

    private void UpdateCampaign(CampaignData data, bool onStart)
    {
        if(!onStart)
        {            
            CampaignData = data; //todo: don't double load
            _userConfiguration.CurrentCampaignID = data.Id;
            ResourceExtensions.Save(_userConfiguration);
        }
                
        var filePath = GetCampaignFilePath(CampaignData.Id);
        var storedCampaign = ResourceExtensions.Load<CampaignData>(filePath);
        if (storedCampaign == null)
        {
            // the file exists but won't load: move it aside instead of
            // silently overwriting it with the default campaign
            var backupPath = ResourceExtensions.BackupUnreadable(filePath);
            if (backupPath != null)
            {
                ShowLoadFailure($"The saved campaign file couldn't be read:\n{filePath}\n\nIt was moved aside as:\n{backupPath}\n\nStarting from the default campaign.");
            }
            CampaignData.Save(filePath);
            var reloaded = ResourceExtensions.Load<CampaignData>(filePath);
            // if the round-trip fails (locked/unwritable file), keep the live
            // in-memory campaign instead of nulling it out
            if (reloaded != null) CampaignData = reloaded;
            else GD.PushError($"couldn't re-read campaign after saving {filePath}; continuing with in-memory data");
        }
        else
        {
            CampaignData = storedCampaign;
        }
        var runState = GetNode<RunState>("/root/RunState");
        runState.Campaign = CampaignData;
        EmitSignal(SignalName.UpdateCurrentCampaign, new SignalWrapper<CampaignData>(CampaignData));
        CampaignData.Changed += HandleCampaignDataChanged;

        // show when this campaign last hit disk (covers returning from battle,
        // where the save happened before this screen existed)
        if (SaveStatusLabel != null && Godot.FileAccess.FileExists(filePath))
        {
            var mtime = DateTimeOffset.FromUnixTimeSeconds((long)Godot.FileAccess.GetModifiedTime(filePath)).ToLocalTime();
            SaveStatusLabel.Text = $"Saved {mtime:HH:mm:ss}";
        }
    }

    private void HandleCampaignDataChanged()
    {
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

    private SceneTreeTimer _saveTimer;
    private Task ReceiveSaveMessage(IMessage message)
    {
        if (!(message is IMessage<SaveMessage> saveMessage)) return Task.CompletedTask;
        // hop to the main thread: this runs on a MessageRouter worker task,
        // and SceneTree/ResourceSaver aren't safe to touch from there
        CallDeferred(MethodName.ScheduleSave);
        return Task.CompletedTask;
    }

    private async void ScheduleSave()
    {
        if (_saveTimer != null) return;
        if (SaveStatusLabel != null) SaveStatusLabel.Text = "Saving…";
        _saveTimer = GetTree().CreateTimer(SaveTimeWindowSeconds);
        await ToSignal(_saveTimer, SceneTreeTimer.SignalName.Timeout); // adjust timing later
        _saveTimer = null;
        SaveNow();
    }

    private void SaveNow()
    {
        if (CampaignData == null) return;
        CampaignData.Save(GetCampaignFilePath(CampaignData.Id));
        if (SaveStatusLabel != null) SaveStatusLabel.Text = $"Saved {DateTime.Now:HH:mm:ss}";
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            // commit any text edit still holding focus, then flush so the
            // debounce window can't drop a save on quit
            GetViewport()?.GuiReleaseFocus();
            SaveNow();
        }
    }

    private async Task ReceiveCampaignUpdate(IMessage message)
    {
        if (message is not IMessage<CampaignUpdate> campaignMessage) return;
        await Task.Run(() =>
        {            
            CallDeferred(MethodName.UpdateCampaign, campaignMessage.Value.CampaignData, false);
        });
    }

    private void ShowLoadFailure(string message)
    {
        var dialog = new AcceptDialog
        {
            Title = "Campaign Load Failed",
            DialogText = message,
        };
        AddChild(dialog);
        dialog.PopupCentered();
    }

    private string GetCampaignFilePath(string campaignID) => Configuration.CampaignFolder + $"{campaignID}.tres";

    public void AddEncounter(Encounter encounter)
    {
        if (CampaignData == null) throw new Exception("campaign shouldn't be null");
        if (encounter == null) throw new ArgumentNullException("encounter should be defined");
        CampaignData.Encounters.Add(encounter);
        EmitSignal(SignalName.UpdateCurrentCampaign, new SignalWrapper<CampaignData>(CampaignData));

        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}

public struct SaveMessage
{   
}

public struct CampaignUpdate
{
    public CampaignData CampaignData { get; set; }
}
