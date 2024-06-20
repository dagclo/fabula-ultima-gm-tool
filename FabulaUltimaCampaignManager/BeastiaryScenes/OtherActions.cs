using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class OtherActions : VBoxContainer, IBeastAttribute
{
    [Export]
    public PackedScene ActionScene { get; set; }

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var child in this.FindChildren("*", owned: false).Where(c => c is OtherActionEntry))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        this.Visible = false;
    }


    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach (var child in this.FindChildren("*", owned: false).Where(c => c is OtherActionEntry))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        if (!beastTemplate.Actions.Any()) return;
        this.Visible = true;
        foreach (var action in beastTemplate.Actions)
        {
            var scene = ActionScene.Instantiate<OtherActionEntry>();
            scene.Action = action;
            this.AddChild(scene);
        }
    }
}
