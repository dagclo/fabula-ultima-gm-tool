using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class DeleteConfirmButton : Button, IBeastAttribute
{
    private IBeastTemplate _beast;
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beast = beastTemplate;
    }

    public void OnPressed()
    {
        // use beast id to delete from database
        BeastTemplateAction.Invoke(new HashSet<BeastEntryNode.Action> { BeastEntryNode.Action.DELETE });
    }
}
