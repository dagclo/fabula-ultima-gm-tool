using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpecialAttackTextEdit : TextEdit
{
    private SkillTemplate _skill;

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool editable)
    {
        this.Text = signal.Value.Text;
        this.Editable = editable;
        _skill = signal.Value;
    }

    public void HandleTextChanged(string newText)
    {
        if (_skill == null) return;
        _skill.Text = newText;        
    }
}
