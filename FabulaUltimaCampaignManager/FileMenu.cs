using Godot;
using System;

public partial class FileMenu : PopupMenu
{
    [Export]
    public PopupPanel ChooseCampaignPanel { get; set; }

    public void OnOptionPressed(int index)
    {
        var itemText = GetItemText(index);
        switch (itemText)
        {
            case "Open Campaign":
                ChooseCampaignPanel.Show();
                break;
        }
    }
}
