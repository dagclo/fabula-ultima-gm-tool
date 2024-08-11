using FabulaUltimaNpc;
using Godot;

public partial class IsSpecialAttack : CheckButton, ISkillReader
{


    public void HandleSkillChanged(SkillTemplate skill)
    {
        this.SetPressedNoSignal(skill.OtherAttributes?.IsSpecialAttack == true);
    }
}
