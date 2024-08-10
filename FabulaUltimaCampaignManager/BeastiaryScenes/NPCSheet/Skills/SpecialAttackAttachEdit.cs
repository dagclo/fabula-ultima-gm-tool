using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpecialAttackAttachEdit : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.Visible = false;
	}

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal)
    {
        this.Visible = signal.Value.OtherAttributes?.IsSpecialAttack ?? false;
    }
}
