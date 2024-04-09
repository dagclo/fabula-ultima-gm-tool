using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class BeastSkills : PanelContainer, ISkillReader
{	
    public SkillTemplate Skill { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Visible = false;
		if (Skill == null) return;
		foreach(var child in this.FindChildren("*", "Label").Where(c => c is ISkillReader))
		{
			var skillReceiver = child as ISkillReader;
			skillReceiver.HandleSkillChanged(Skill);
		}
        this.Visible = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleSkillChanged(SkillTemplate skill)
    {
        if (skill == null) return;
        Skill = skill;
        foreach (var child in this.FindChildren("*", "Label").Where(c => c is ISkillReader))
        {
            var skillReceiver = child as ISkillReader;
            skillReceiver.HandleSkillChanged(Skill);
        }
        this.Visible = true;
    }
}
