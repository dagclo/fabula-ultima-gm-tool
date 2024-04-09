using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;

public partial class NpcName : LineEdit, INpcReader
{
	private NpcInstance _instance;
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

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
        this.Text = _instance?.InstanceName ?? string.Empty;
    }

	public void OnTextSubmitted(string text)
	{
        if (_instance == null) return;
        _instance.InstanceName = text;
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
    
}
