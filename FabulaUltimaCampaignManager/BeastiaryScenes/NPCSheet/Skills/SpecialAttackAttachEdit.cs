using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpecialAttackAttachEdit : HBoxContainer
{
    private SkillTemplate _skill;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Visible = false;
	}

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool _)
    {
        _skill = signal.Value;
        HandleToggled(_skill.OtherAttributes?.IsSpecialAttack ?? false);
    }

    public void HandleToggled(bool on)
    {
        this.Visible = on;
    }
}
