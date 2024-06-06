using FabulaUltimaNpc;
using Godot;
using System;
using System.IO;

public partial class DescriptionImage : TextureRect, IBeastAttribute
{
    private IBeastTemplate _beastTemplate = null;

    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        if (string.IsNullOrWhiteSpace(_beastTemplate.ImageFile)) return;
        const string RES = "res://";
        
        if (!_beastTemplate.ImageFile.StartsWith(RES))
        {
            const string targetFolder = "Database/Images/";
            var fileName = Path.GetFileName(_beastTemplate.ImageFile);
            var targetpath = $"{RES}{targetFolder}{fileName}";
            string targetPathAbsolute;
            using (var godotFile = Godot.FileAccess.Open(targetpath, Godot.FileAccess.ModeFlags.WriteRead))
            {
                targetPathAbsolute = godotFile.GetPathAbsolute();
            }
            
            File.Copy(_beastTemplate.ImageFile, targetPathAbsolute, true);
            HandleImageSet(targetpath);
            return;
        }
		var image = Image.LoadFromFile(_beastTemplate.ImageFile);        
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        _beastTemplate.ImageFile = imageFileName;
        Save?.Invoke(false);
    }
}
