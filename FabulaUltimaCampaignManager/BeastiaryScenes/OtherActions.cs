using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class OtherActions : VBoxContainer, IBeastAttribute
{
    [Export]
    public PackedScene ActionScene { get; set; }

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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
