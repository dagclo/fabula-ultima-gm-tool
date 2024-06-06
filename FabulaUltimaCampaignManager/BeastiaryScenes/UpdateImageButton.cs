using FabulaUltimaNpc;
using Godot;
using System;

public partial class UpdateImageButton : Button, IBeastAttribute
{
    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = !beastTemplate.Immutable;
    }
}
