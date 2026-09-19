using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;

public partial class CharacterNameEdit : LineEdit, IPlayerAttribute
{
    private PlayerData _player;
    private MessagePublisher<SaveMessage> _messagePublisher;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
        this.FocusExited += () => OnTextSubmitted(this.Text); // commit on click-away, not just Enter
    }

    public void SetPlayer(PlayerData player)
    {
        _player = player;
        this.Text = _player.CharacterName;
    }

    public void OnTextSubmitted(string newText)
    {
        if (_player == null || (_player.CharacterName ?? "") == newText) return;
        _player.CharacterName = newText;
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
