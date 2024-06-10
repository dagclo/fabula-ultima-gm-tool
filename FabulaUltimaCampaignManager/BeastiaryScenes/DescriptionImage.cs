using FabulaUltimaNpc;
using Godot;
using System;

public partial class DescriptionImage : TextureRect, IBeastAttribute
{
    private IBeastTemplate _beastTemplate = null;

    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        if (string.IsNullOrWhiteSpace(_beastTemplate.ImageFile)) return;

        if (_beastTemplate.ImageFile.CopyToResourceFolder(out var newPath))
        {
            HandleImageSet(newPath);
            return;
        }
        Texture2D texture = 
            FirstProject.ResourceExtensions.Load<Texture2D>(_beastTemplate.ImageFile) ?? 
            LoadFromFile(_beastTemplate.ImageFile);
		this.Texture = texture;
    }

    private static ImageTexture LoadFromFile(string filePath)
    {
        var image = Image.LoadFromFile(filePath);        
		return ImageTexture.CreateFromImage(image);
    }

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        imageFileName.CopyToResourceFolder(out var newPath);
        _beastTemplate.ImageFile = newPath;
        Save?.Invoke(false);
    }
}