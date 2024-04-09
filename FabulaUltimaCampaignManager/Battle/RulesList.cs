using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class RulesList : VBoxContainer, INpcReader
{
    [Export]
    public PackedScene SkillScene { get; set; }

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
        foreach (var child in this.FindChildren("Skill", "PanelContainer"))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (!npc.Template.Skills.Any(s => s.IsSpecialRule)) return;
        var beastTemplate = npc.Template;
        foreach (var skill in beastTemplate.Skills.Where(s => s.IsSpecialRule))
        {
            var scene = SkillScene.Instantiate<BeastSkills>();
            this.AddChild(scene);
            scene.HandleSkillChanged(skill);
        }
    }
}
