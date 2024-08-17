using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteButton : Button, IBeastAttribute
{
    [Export]
    public PopupPanel DeleteDialog { get; set; }
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }    

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = beastTemplate.CanBeDeleted;
    }

    public void OnClick()
    {
        this.DeleteDialog.Visible = true;
    }
}
