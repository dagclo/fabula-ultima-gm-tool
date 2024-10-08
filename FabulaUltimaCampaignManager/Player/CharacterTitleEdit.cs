using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;

public partial class CharacterTitleEdit : LineEdit, IPlayerAttribute
{
    private PlayerData _player;
    private MessagePublisher<SaveMessage> _messagePublisher;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

    public void SetPlayer(PlayerData player)
    {
        _player = player;
        this.Text = _player.CharacterTitle;
    }

    public void OnTextSubmitted(string newText)
    {
        _player.CharacterTitle = newText;
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
