using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Campaign : Control
{
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
    public CampaignData CampaignData { get; set; }

    [Export]
    public Configuration Configuration { get; set; }

    [Signal]
    public delegate void UpdateCurrentCampaignEventHandler(SignalWrapper<CampaignData> campaign);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        if(Configuration != null)
        {
            Configuration.MakeDirectories();
        }

        if (CampaignData != null)
        {
            var filePath = GetCampaignFilePath();
            var storedCampaign = ResourceExtensions.Load<CampaignData>(filePath);
            if (storedCampaign == null)
            {   
                CampaignData.Save(filePath);
                CampaignData = ResourceExtensions.Load<CampaignData>(filePath);
            }
            else
            {   
                CampaignData = storedCampaign;
            }
            var runState = GetNode<RunState>("/root/RunState");
            runState.Campaign = CampaignData;
            EmitSignal(SignalName.UpdateCurrentCampaign, new SignalWrapper<CampaignData>(CampaignData));
        }

        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<SaveMessage>(this.ReceiveSaveMessage);
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

    private async Task ReceiveSaveMessage(IMessage message)
    {
        if (!(message is IMessage<SaveMessage> saveMessage)) return;                
        var savePath = GetCampaignFilePath();        ;
        await Task.Run(() => CampaignData.Save(savePath));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    private string GetCampaignFilePath() => Configuration.CampaignFolder + $"{CampaignData.Id}.tres";

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
