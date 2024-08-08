using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class BasicAttackInputs : VBoxContainer, IBeastAttribute
{
    private ICollection<BasicAttackTemplate> _basicAttacks;

    [Export]
    public PackedScene BasicAttackInputScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _basicAttacks = beastTemplate.Model.BasicAttacks;
    }
}
