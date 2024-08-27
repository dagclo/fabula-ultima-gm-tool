using FirstProject.Messaging;
using Godot;

public partial class EndEncounterButton : Button
{
    private MessagePublisher<EncounterEnd> _messagePublisher;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterEnd>();
    }

	public void OnButtonPressed()
	{
        _messagePublisher.Publish(new EncounterEnd().AsMessage());
    }
}
