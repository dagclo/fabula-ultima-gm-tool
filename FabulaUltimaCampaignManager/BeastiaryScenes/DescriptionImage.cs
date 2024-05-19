using FabulaUltimaNpc;
using Godot;
using System;

public partial class DescriptionImage : TextureRect, IBeastAttribute
{
    private IBeastTemplate _beastTemplate = null;

    public Action Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        if (string.IsNullOrWhiteSpace(_beastTemplate.ImageFile)) return;
		var image = Image.LoadFromFile(_beastTemplate.ImageFile);
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        _beastTemplate.ImageFile = imageFileName;
        Save?.Invoke();
    }
}
