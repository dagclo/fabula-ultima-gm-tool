using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AttackNameLineEdit : LineEdit
{
    private BasicAttackTemplate _basicAttack;

    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
	{
		var attack = signal.Value;
		_basicAttack = attack;
	}

	public void HandleTextChanged(string text)
	{
		_basicAttack.Name = text;
	}
}
