using Godot;
using System;

namespace FirstProject
{
    public static class ResourceExtensions
    {
        // a resource file that exists but won't load is presumed corrupt: move it
        // aside rather than letting callers overwrite it with defaults
        // returns the backup path, or null if there was no file to move
        public static string BackupUnreadable(string path)
        {
            if (!Godot.FileAccess.FileExists(path)) return null;
            var backupPath = $"{path}.unreadable-{DateTime.Now:yyyyMMdd-HHmmss}";
            DirAccess.RenameAbsolute(path, backupPath);
            GD.PushError($"resource file {path} exists but couldn't be loaded; moved to {backupPath}");
            return backupPath;
        }

        public static void Save(this Resource resource, string savePath = "")
        {   
            var error = ResourceSaver.Save(resource, savePath);
            if (error != Error.Ok)
            {
                GD.Print($"error while saving resource {error}");
            }
        }

        public static T Load<T>(string loadPath) where T : Resource
        {
            if(!ResourceLoader.Exists(loadPath)) return default(T);
            var result = ResourceLoader.Load(loadPath);
            return (T) result;
        }
    }
}
