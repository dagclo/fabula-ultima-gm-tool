using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SkillNameEdit : LineEdit
{
	public void HandleSkillSet(SignalWrapper<SkillTemplate> signal)
	{
		this.Text = signal.Value.Name;
	}
}
