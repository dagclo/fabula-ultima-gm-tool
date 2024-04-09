using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class AttackRoll : Label, IAttackReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadAttack(BasicAttackTemplate attack)
    {
		var attackText = $"[{attack.Attribute1.ShortenAttribute()} + {attack.Attribute2.ShortenAttribute()}]";
		if(attack.AttackMod > 0)
		{
			attackText = $"{attackText} + {attack.AttackMod}";
		}
		this.Text = attackText;
    }
}
