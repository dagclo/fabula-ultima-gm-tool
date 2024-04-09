using FabulaUltimaNpc;
using Godot;

public partial class DamageRoll : Label, IAttackReader
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
		string modText;
		if(attack.DamageMod > 0)
		{
            modText = $"[HR + {attack.DamageMod}]";
		}
		else
		{
			modText = "[HR]";
		}
		this.Text = modText;		
    }
}
