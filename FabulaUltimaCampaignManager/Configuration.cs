using Godot;

namespace FirstProject
{
    public partial class Configuration : Resource
    {

        [Export]
        public string CampaignFolder { get; set; }

        [Export]
        public string DatabaseFilePath { get; set; }

        [Export]
        public bool BackgroundMusicEnabled { get; set; }

        internal void MakeDirectories()
        {
            DirAccess.MakeDirRecursiveAbsolute(CampaignFolder);
        }
    }
}
