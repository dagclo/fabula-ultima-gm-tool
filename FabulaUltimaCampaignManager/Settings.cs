using FirstProject;
using Godot;
using System;

public partial class Settings : PopupMenu
{
    [Export]
    public Configuration _configuration { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetItemChecked(0, _configuration.BackgroundMusicEnabled);
    }

    public void OnOptionPressed(int index)
    {
        var itemText = GetItemText(index);
        
        switch (itemText)
        {
            case "Play Background Music":                
                _configuration.BackgroundMusicEnabled = !_configuration.BackgroundMusicEnabled;
                SetItemChecked(index, _configuration.BackgroundMusicEnabled);
                ResourceExtensions.Save(_configuration);
                break;
        }
    }
}
