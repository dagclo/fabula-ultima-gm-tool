using FabulaUltimaNpc;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SpecialRules : VBoxContainer, IBeastAttribute
{
    [Export]
    public bool OnlySpecialRules { get; set; }

    [Export]
    public PackedScene SkillNodeScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach (var child in this.FindChildren("*", owned: false).Where(c => c is BeastSkills))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        IEnumerable<SkillTemplate> specialSkills = beastTemplate.Skills;

        if (OnlySpecialRules)
        {
            specialSkills = specialSkills.Where(s => s.IsSpecialRule).ToList();
        }

        if (!specialSkills.Any()) return;
        this.Visible = true;

        foreach (var skill in specialSkills)
        {
            var scene = SkillNodeScene.Instantiate<BeastSkills>();
            scene.Skill = skill;
            this.AddChild(scene);
        }
    }
}
