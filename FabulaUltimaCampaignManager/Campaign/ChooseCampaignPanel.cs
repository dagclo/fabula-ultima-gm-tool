using Godot;

public partial class ChooseCampaignPanel : PopupPanel
{
    [Signal]
    public delegate void OnVisibilityChangedEventHandler(bool isVisible);

    public void HandleVisibilityChanged()
    {
        EmitSignal(SignalName.OnVisibilityChanged, this.Visible);
    }

    public void HandleCampaignChosen()
    {
        this.Hide();

    }
}
