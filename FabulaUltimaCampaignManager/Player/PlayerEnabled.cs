using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;

public partial class PlayerEnabled : CheckButton, IPlayerAttribute
{
    private PlayerData _player;
    private MessagePublisher<SaveMessage> _messagePublisher;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

    public void SetPlayer(PlayerData data)
    {
		_player = data;
        this.SetPressedNoSignal(_player.Enabled);
    }

    public void HandleToggled(bool on)
    {
        _player.Enabled = on;
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
