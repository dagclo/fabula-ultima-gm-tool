using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class CurrentSpellList : VBoxContainer, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public PackedScene AddSpellScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }
}
