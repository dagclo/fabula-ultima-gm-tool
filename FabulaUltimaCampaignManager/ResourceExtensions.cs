using Godot;

namespace FirstProject
{
    public static class ResourceExtensions
    {
        public static void Save(this Resource resource, string savePath = "")
        {   
            var error = ResourceSaver.Save(resource, savePath);
            if (error != Error.Ok)
            {
                GD.Print($"error while save campaign {error}");
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
