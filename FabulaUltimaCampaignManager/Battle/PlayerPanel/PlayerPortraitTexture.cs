using Godot;
using FirstProject.Campaign;

public partial class PlayerPortraitTexture : TextureRect
{
	private void LoadTexture(string imageFile)
	{
		if(string.IsNullOrWhiteSpace(imageFile)) return;
		var image = Image.LoadFromFile(imageFile);        
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}
	public void ReadPlayer(PlayerData player)
    {
        LoadTexture(player.PortraitFile);
    }
}
