using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;

public partial class EncounterList : Node
{
	[Export]
	public PackedScene PackedEncounter { get; set; }

    [Signal]
    public delegate void LoadEncounterEventHandler(Encounter encounter);

    [Signal]
    public delegate void DeleteEncounterEventHandler(Encounter encounter);

    private CampaignData _campaign;
    private MessagePublisher<SaveMessage> _messagePublisher;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateEncounter(SignalWrapper<CampaignData> signalWrapper)
    {	
		// remove any existing children
		foreach(var child in this.GetChildren())
		{
            this.RemoveChild(child);
			child.QueueFree();
        }

		_campaign = signalWrapper.Value;
		if (_campaign == null) return;
		foreach(var encounter in _campaign.Encounters)
		{
			var node = PackedEncounter.Instantiate<EncounterEntry>();
            this.AddChild(node);
            node.SetEncounter(encounter);
			node.OnSave += SaveCampaign;
			node.OnDeleteEncounter += OnDeleteEncounter;
			node.OnLoadEncounter += OnLoadEncounter;
        }
    }

    private void OnDeleteEncounter(Encounter encounter, EncounterEntry entry)
    {
		var targetEncounterIndex = _campaign?.Encounters.IndexOf(encounter) ?? -1;
		if (targetEncounterIndex < 0) return;
		_campaign.Encounters.RemoveAt(targetEncounterIndex);
		this.RemoveChild(entry);
		entry.QueueFree();
		SaveCampaign();
		EmitSignal(SignalName.DeleteEncounter, encounter);
    }

    private void SaveCampaign()
	{
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

	private void OnLoadEncounter(Encounter encounter)
	{
		EmitSignal(SignalName.LoadEncounter, encounter);
	}
}
