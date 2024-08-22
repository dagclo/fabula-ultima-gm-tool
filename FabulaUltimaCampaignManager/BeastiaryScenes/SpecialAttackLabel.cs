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

    public void ReadAttack(BasicAttackTemplate attack)
    {
		if (attack.AttackSkills == null) return;
		var stringBuilder = new StringBuilder();
		bool first = true;
		foreach(var skill in attack.AttackSkills.Where(s => s.OtherAttributes?.IsSpecialAttack == true))
		{
			if (!first)
			{
                stringBuilder.Append("; ");				
            }
            first = false;
            stringBuilder.Append(skill.Text);
        }
		this.Text = stringBuilder.ToString();
    }
}
