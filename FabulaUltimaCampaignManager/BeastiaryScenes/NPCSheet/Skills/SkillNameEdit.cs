using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SkillNameEdit : LineEdit
{
	public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool editable)
	{
		this.Text = signal.Value.Name;
		this.Editable = editable;
	}
}
