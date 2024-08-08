using FabulaUltimaNpc;
using Godot;
using System.Linq;
using System.Text;

public partial class SpecialAttackLabel : Label, IAttackReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Text = string.Empty;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadAttack(BasicAttackTemplate attack)
    {
		if (attack.AttackSkills == null) return;
		var stringBuilder = new StringBuilder();
		
		foreach(var skill in attack.AttackSkills.Where(s => s.OtherAttributes?.IsSpecialAttack == true))
		{	
            stringBuilder.Append(" and ");
            stringBuilder.Append(skill.Text);
        }
		this.Text = stringBuilder.ToString();
    }
}
