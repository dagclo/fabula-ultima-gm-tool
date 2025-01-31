using FirstProject.Campaign;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FirstProject
{
    public partial class Configuration : Resource
    {

        [Export]
        public string CampaignFolder { get; set; }

        [Export]
        public string DatabaseFilePath { get; set; }

        [Export]
        public string CurrentCampaignID { get; set; }

        [Export]
        public bool BackgroundMusicEnabled { get; set; }

        [Export]
        public string[] InstanceNames { get; set; }

        internal IEnumerable<CampaignData> GetCampaigns()
        {
            var filePaths = DirAccess.GetFilesAt(CampaignFolder).Where(p => p.EndsWith(".tres"));
            foreach (var file in filePaths)
            {
                var campaign = ResourceExtensions.Load<CampaignData>(CampaignFolder + file);
                if(campaign != null) yield return campaign;
            }
        }

        internal void MakeDirectories()
        {
            DirAccess.MakeDirRecursiveAbsolute(CampaignFolder);
        }
    }
}
