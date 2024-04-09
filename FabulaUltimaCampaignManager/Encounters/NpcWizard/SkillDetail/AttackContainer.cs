using FabulaUltimaNpc;
using Godot;
using System;
using FabulaUltimaSkillLibrary;

public partial class AttackContainer : HBoxContainer, ISkillReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleSkillChanged(SkillTemplate skill)
    {
		this.Visible = skill.ModifiesAttack();
    }
}
