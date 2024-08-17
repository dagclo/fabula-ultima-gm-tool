using FabulaUltimaNpc;
using Godot;
using System;

public partial class UpdateImageButton : Button, IBeastAttribute
{
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = beastTemplate.CanBeModified;
    }
}
