using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddToEncounterButton : Button, IBeastAttribute
{
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = beastTemplate.CanBeModified && beastTemplate.CanBeDeleted;
    }
}
