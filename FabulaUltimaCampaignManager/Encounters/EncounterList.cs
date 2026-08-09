using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System.Linq;

public partial class EncounterList : Node
{
	[Export]
	public PackedScene PackedEncounter { get; set; }

    [Export]
    public Tag ArchiveTag { get; set; }

	private bool _showArchived = false;

    [Signal]
    public delegate void LoadEncounterEventHandler(Encounter encounter);

    [Signal]
    public delegate void DeleteEncounterEventHandler(Encounter encounter);

    private CampaignData _campaign;
    private MessagePublisher<SaveMessage> _messagePublisher;


    private ConfirmationDialog _deleteConfirm;
    private Encounter _pendingDelete;
    private EncounterEntry _pendingDeleteEntry;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();

        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        _deleteConfirm = new ConfirmationDialog { Title = "Delete Scene" };
        AddChild(_deleteConfirm);
        _deleteConfirm.Confirmed += HandleDeleteConfirmed;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateEncounter(SignalWrapper<CampaignData> signalWrapper)
    {
        _campaign = signalWrapper.Value;
        Refresh();
    }

    private void Refresh()
    {
        if (_campaign == null) return;
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        foreach (var encounter in _campaign.Encounters.Where(e => _showArchived || !e.HasTag(ArchiveTag)))
        {
            var node = PackedEncounter.Instantiate<EncounterEntry>();
            this.AddChild(node);
            node.SetEncounter(encounter);
            node.OnSave += SaveCampaign;
            node.OnDeleteArchiveEncounter += OnDeleteArchiveEncounter;
            node.OnLoadEncounter += OnLoadEncounter;
        }
    }

    private void OnDeleteArchiveEncounter(Encounter encounter, EncounterEntry entry)
    {
		var targetEncounterIndex = _campaign?.Encounters.IndexOf(encounter) ?? -1;
		if (targetEncounterIndex < 0) return;
		if (encounter.HasTag(ArchiveTag))
		{
            // permanent deletion needs a confirmation; archiving is reversible and doesn't
            _pendingDelete = encounter;
            _pendingDeleteEntry = entry;
            var name = string.IsNullOrWhiteSpace(encounter.Name) ? "this scene" : $"\"{encounter.Name}\"";
            _deleteConfirm.DialogText = $"Permanently delete {name}? This can't be undone.";
            _deleteConfirm.PopupCentered();
            return;
		}

		encounter.AddTags(ArchiveTag);
		this.RemoveChild(entry);
		entry.QueueFree();
		SaveCampaign();
		EmitSignal(SignalName.DeleteEncounter, encounter);
    }

    private void HandleDeleteConfirmed()
    {
        if (_pendingDelete == null) return;
        var index = _campaign?.Encounters.IndexOf(_pendingDelete) ?? -1;
        if (index >= 0) _campaign.Encounters.RemoveAt(index);
        if (IsInstanceValid(_pendingDeleteEntry))
        {
            this.RemoveChild(_pendingDeleteEntry);
            _pendingDeleteEntry.QueueFree();
        }
        SaveCampaign();
        EmitSignal(SignalName.DeleteEncounter, _pendingDelete);
        _pendingDelete = null;
        _pendingDeleteEntry = null;
    }

    private void SaveCampaign()
	{
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

	private void OnLoadEncounter(Encounter encounter)
	{
        encounter.RemoveTag(ArchiveTag); // encounters that are loaded can't be deleted
		EmitSignal(SignalName.LoadEncounter, encounter);
        Refresh();
    }

	public void HandleToggleArchive(bool toggleOn)
	{
		_showArchived = toggleOn;
        Refresh();
	}
}
