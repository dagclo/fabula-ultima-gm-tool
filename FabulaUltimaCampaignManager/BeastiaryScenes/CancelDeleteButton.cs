using Godot;

public partial class CancelDeleteButton : Button
{
    [Export]
    public PopupPanel DeleteBeastDialog { get; set; }

    public void OnPressed()
    {
        DeleteBeastDialog.Visible = false;
    }
}
