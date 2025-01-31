using Godot;

public partial class FileMenu : PopupMenu
{
    [Export]
    public PopupPanel ChooseCampaignPanel { get; set; }

    [Export]
    public PopupPanel NewCampaignPanel { get; set; }

    public void OnOptionPressed(int index)
    {
        var itemText = GetItemText(index);
        switch (itemText)
        {
            case "Load Campaign":
                ChooseCampaignPanel.Show();
                break;
            case "Create New Campaign":
                NewCampaignPanel.Show();
                break;
        }
    }
}
