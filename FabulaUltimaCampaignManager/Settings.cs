using FirstProject;
using Godot;

public partial class Settings : PopupMenu
{
    [Export]
    public Configuration Configuration { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetItemChecked(0, Configuration.BackgroundMusicEnabled);
    }

    public void OnOptionPressed(int index)
    {
        var itemText = GetItemText(index);
        
        switch (itemText)
        {
            case "Play Background Music":                
                Configuration.BackgroundMusicEnabled = !Configuration.BackgroundMusicEnabled;
                SetItemChecked(index, Configuration.BackgroundMusicEnabled);
                ResourceExtensions.Save(Configuration);
                break;
        }
    }
}
