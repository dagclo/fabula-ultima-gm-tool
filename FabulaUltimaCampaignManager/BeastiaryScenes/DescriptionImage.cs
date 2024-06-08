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

        if (_beastTemplate.ImageFile.CopyToResourceFolder(out var newPath))
        {
            HandleImageSet(newPath);
            return;
        }
       
		var image = Image.LoadFromFile(_beastTemplate.ImageFile);        
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }

   

	public void HandleImageSet(string imageFileName)
	{
        if (string.IsNullOrWhiteSpace(imageFileName)) return;
        imageFileName.CopyToResourceFolder(out var newPath);
        _beastTemplate.ImageFile = newPath;
        Save?.Invoke(false);
    }
}

public static class FileExtensions
{
    public static bool CopyToResourceFolder(this string originalFile, out string targetpath)
    {
        const string RES = "res://";
        targetpath = string.Empty;

        if (originalFile.StartsWith(RES)) return false; // file is already copied presumably
        if (!File.Exists(originalFile)) return true; // set image file path to empty if we can't find the file

        string targetFolder = $"{RES}Database/Images";
        var fileName = Path.GetFileName(originalFile);
        targetpath = $"{targetFolder}/{fileName}";
        using var directory = DirAccess.Open(targetFolder);
        directory.Copy(originalFile, targetpath);       
         
        return true;
    }
}
