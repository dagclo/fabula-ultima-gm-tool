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
        var children = this.GetChildren();
        foreach (var child in children)
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        foreach(var attack in beastTemplate.AllAttacks.Where(a => a.IsEquipmentAttack))
        {
            var attackScene = BasicAttackScene.Instantiate<BasicAttack>();
            attackScene.BasicAttackObject = attack;
            this.AddChild(attackScene);
        }
    }
}
