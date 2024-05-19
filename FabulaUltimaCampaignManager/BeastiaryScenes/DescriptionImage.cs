using FabulaUltimaNpc;
using Godot;

public partial class DescriptionImage : TextureRect, IBeastAttribute
{

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
		if(string.IsNullOrWhiteSpace(beastTemplate.ImageFile)) return;
		var image = Image.LoadFromFile(beastTemplate.ImageFile);
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        var image = Image.LoadFromFile(imageFileName);
        var texture = ImageTexture.CreateFromImage(image);
        this.Texture = texture;
    }
}
