using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddToEncounterButton : Button, IBeastAttribute
{
    Action<ISet<BeastEntryNode.Action>> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = !beastTemplate.Immutable;
    }
}
