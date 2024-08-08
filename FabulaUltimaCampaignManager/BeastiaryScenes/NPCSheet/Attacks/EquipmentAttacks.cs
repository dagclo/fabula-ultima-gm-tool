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
        // get set of weapon ids
        foreach(var attack in beastTemplate.AllAttacks.Where(a => a.e) // use weapon ids to detect equipment attacks
        {

        }
    }
}
