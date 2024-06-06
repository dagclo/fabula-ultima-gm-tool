using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteButton : Button, IBeastAttribute
{
    [Export]
    public PopupPanel DeleteDialog { get; set; }
    public Action Save { get; set; }
    Action<bool> IBeastAttribute.Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = !beastTemplate.Immutable;
    }

    public void OnClick()
    {
        this.DeleteDialog.Visible = true;
    }
}
