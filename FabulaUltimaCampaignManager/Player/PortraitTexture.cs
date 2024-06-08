using FirstProject.Campaign;
using Godot;
using FirstProject.Messaging;

public partial class PortraitTexture : TextureRect, IPlayerAttribute
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
		LoadTexture();
    }

	private void LoadTexture()
	{
		if(string.IsNullOrWhiteSpace(_player.PortraitFile)) return;
		var image = Image.LoadFromFile(_player.PortraitFile);        
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        imageFileName.CopyToResourceFolder(out var newPath);
        _player.PortraitFile= newPath;
		LoadTexture();
		_messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
