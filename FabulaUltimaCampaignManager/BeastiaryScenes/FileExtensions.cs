using Godot;
using System.IO;

public static class FileExtensions
{
    public static bool CopyToResourceFolder(this string originalFile, out string targetpath)
    {
        const string RES = "res://";
        string targetFolder = $"{RES}Database/Images";
        return originalFile.CopyFile(targetFolder, out targetpath);
    }

    public static bool CopyToUserFolder(this string originalFile, out string targetpath)
    {
        const string USER_FOLDER = "user://";
        string targetFolder = $"{USER_FOLDER}/LocalFiles";
        if(!DirAccess.DirExistsAbsolute(targetFolder)){
            DirAccess.MakeDirAbsolute(targetFolder);
        }
        return originalFile.CopyFile(targetFolder, out targetpath);
    }

    public static bool CopyFile(this string originalFile, string baseFolder, out string targetPath)
    {
        targetPath = string.Empty;
        if (originalFile.StartsWith(baseFolder)) return false; // file is already copied presumably
        if (!File.Exists(originalFile)) return true; // set image file path to empty if we can't find the file
        var fileName = Path.GetFileName(originalFile);
        targetPath = $"{baseFolder}/{fileName}";
        using var directory = DirAccess.Open(baseFolder);
        directory.Copy(originalFile, targetPath);                
        return true;
    }

}
