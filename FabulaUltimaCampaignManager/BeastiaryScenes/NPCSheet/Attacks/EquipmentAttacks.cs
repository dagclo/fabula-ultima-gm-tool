using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EquipmentAttacks : VBoxContainer, IBeastAttribute
{
    [Export]
    public PackedScene BasicAttackScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach(var attack in beastTemplate.AllAttacks.Where(a => a.e)
        {

        }
    }
}
