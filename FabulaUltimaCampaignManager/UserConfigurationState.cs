using FirstProject;
using Godot;

namespace FabulaUltimaGMTool
{
    public partial class UserConfigurationState : Node
    {
        public UserConfigurationData UserConfigurationData { get; set; }

        public UserConfigurationState()
        {
            var configuration = ResourceExtensions.Load<Configuration>("res://configuration.tres");
            var userConfigurationFilePath = $"{configuration.UserConfigurationFolder}userConfiguration.tres";
            if (!FileAccess.FileExists(userConfigurationFilePath))
            {
                configuration.MakeConfigurationDirectories();
                var userData = new UserConfigurationData
                {
                    BackgroundMusicEnabled = configuration.BackgroundMusicEnabled,
                    InstanceNames = configuration.InstanceNames,
                };
                ResourceExtensions.Save(userData, userConfigurationFilePath);                
            }
            UserConfigurationData = ResourceExtensions.Load<UserConfigurationData>(userConfigurationFilePath);
        }
    }
}
