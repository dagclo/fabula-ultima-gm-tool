using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpecialAttackCheckBox : CheckButton
{
    private SkillTemplate _skill;

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool isEditable)
	{
		_skill = signal.Value;
		this.SetPressedNoSignal(_skill.OtherAttributes.IsSpecialAttack == true);
		this.Disabled = !isEditable;
	}

	public void HandleToggled(bool on)
	{
		_skill.OtherAttributes.IsSpecialAttack = on;
		_skill.IsSpecialRule = !(_skill.OtherAttributes.IsSpecialAttack ?? false);
	}
}
