using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteButton : Button
{
    [Export]
    public PopupPanel DeleteDialog { get; set; }
    public Action Save { get; set; }

    public void OnClick()
    {
        this.DeleteDialog.Visible = true;
    }
}
