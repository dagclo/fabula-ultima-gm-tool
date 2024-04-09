using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class OtherActionList : VBoxContainer, INpcReader
{
    [Export]
    public PackedScene ActionScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
        foreach (var child in this.FindChildren("OtherAction", "PanelContainer"))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (!npc.Template.Actions.Any()) return;
        var beastTemplate = npc.Template;
        foreach (var action in beastTemplate.Actions)
        {
            var scene = ActionScene.Instantiate<OtherActionEntry>();
            this.AddChild(scene);
            scene.UpdateAction(action);
        }
    }
}
