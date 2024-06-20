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
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void SetPlayer(PlayerData player)
    {
        _player = player;
        this.Text = _player.CharacterName;
    }

    public void OnTextSubmitted(string newText)
    {
        _player.CharacterName = newText;
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
