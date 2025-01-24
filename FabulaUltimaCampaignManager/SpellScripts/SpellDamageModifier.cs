using FabulaUltimaNpc;
using Godot;
using System;

public partial class SpellDamageModifier : Label, ISpellReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.Visible = false;
	}

    public void Read(SpellTemplate spellTemplate)
    {
        if (spellTemplate.DamageModifier == null) return;
        this.Text = $"[HR + {spellTemplate.DamageModifier}] {spellTemplate.DamageType.Name} damage";
        this.Visible = true;
    }

    public void Read(IBeastTemplate beast)
    {
        // do nothing
    }
}
