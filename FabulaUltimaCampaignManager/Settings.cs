using FabulaUltimaGMTool;
using FirstProject;
using Godot;

public partial class Settings : PopupMenu
{
    private UserConfigurationData _userConfiguration;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _userConfiguration = GetNode<UserConfigurationState>("/root/UserConfigurationState").UserConfigurationData;
        SetItemChecked(0, _userConfiguration.BackgroundMusicEnabled);        
    }

    public void OnOptionPressed(int index)
    {
        var itemText = GetItemText(index);
        
        switch (itemText)
        {
            case "Play Background Music":
                _userConfiguration.BackgroundMusicEnabled = !_userConfiguration.BackgroundMusicEnabled;
                SetItemChecked(index, _userConfiguration.BackgroundMusicEnabled);
                ResourceExtensions.Save(_userConfiguration);
                break;
        }
    }
}
