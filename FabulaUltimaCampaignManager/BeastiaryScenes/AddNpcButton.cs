using FirstProject.Messaging;
using Godot;

public partial class AddNpcButton : Button
{
    private MessagePublisher<BeastiaryRefreshMessage> _messagePublisher;

    [Export]
    public PackedScene AddNpcScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");        
        _messagePublisher = messageRouter.GetPublisher<BeastiaryRefreshMessage>();
    }

	public void HandlePressed()
	{
		var npcScene = AddNpcScene?.Instantiate<NpcSheet>();
        this.AddChild(npcScene);
        npcScene.Closing += () => OnNpcClose(npcScene);
    }

    private void OnNpcClose(NpcSheet npcScene)
    {
        RemoveChild(npcScene);
        npcScene.QueueFree();
        _messagePublisher.Publish(new BeastiaryRefreshMessage().AsMessage());
    }
}
