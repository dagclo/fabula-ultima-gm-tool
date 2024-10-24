using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellCastRoll : Label, ISpellReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Read(SpellTemplate spellTemplate)
	{
		string roll;
		if (spellTemplate.IsOffensive)
		{
			roll = $"[{spellTemplate.Attribute1.ShortenAttribute()} + {spellTemplate.Attribute2.ShortenAttribute()}]";
		}
		else
		{
			roll = string.Empty;
		}
		this.Text = roll;
	}

	public void Read(IBeastTemplate beast)
	{
		// do nothing

	}
}
