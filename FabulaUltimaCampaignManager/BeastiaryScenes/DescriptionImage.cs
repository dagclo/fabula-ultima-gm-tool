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

        if (CopyToResourceFolder(_beastTemplate.ImageFile, out var newPath))
        {
            HandleImageSet(newPath);
            return;
        }
       
		var image = Image.LoadFromFile(_beastTemplate.ImageFile);        
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }

    private static bool CopyToResourceFolder(string originalFile, out string targetpath)
    {
        const string RES = "res://";
        targetpath = string.Empty;

        if (originalFile.StartsWith(RES)) return false; // file is already copied presumably
        if (!File.Exists(originalFile)) return true; // set image file path to empty if we can't find the file

        const string targetFolder = "Database/Images/";
        var fileName = Path.GetFileName(originalFile);
        targetpath = $"{RES}{targetFolder}{fileName}";
        string targetPathAbsolute;
        using (var godotFile = Godot.FileAccess.Open(targetpath, Godot.FileAccess.ModeFlags.WriteRead))
        {
            targetPathAbsolute = godotFile.GetPathAbsolute(); // get the actual file path
        }

        File.Copy(originalFile, targetPathAbsolute, true);
         
        return true;
    }

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        CopyToResourceFolder(imageFileName, out var newPath);
        _beastTemplate.ImageFile = newPath;
        Save?.Invoke(false);
    }
}
