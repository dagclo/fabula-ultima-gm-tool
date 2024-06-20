using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class Attacks : VBoxContainer, IBeastAttribute
{
    [Export]
    public PackedScene AttackNodeScene { get; set; }

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach(var child in this.FindChildren("*", owned: false).Where(c => c is BasicAttack)) //instantiated scenes aren't "owned"
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        
        foreach(var attack in beastTemplate.AllAttacks)
        {            
            var scene = AttackNodeScene.Instantiate<BasicAttack>();
            scene.BasicAttackObject = attack;
            this.AddChild(scene);
        }
    }
}
