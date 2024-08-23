using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SkillDescriptionTextEdit : TextEdit
{
    private SkillTemplate _skill;

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool editable)
    {
        this.Text = string.IsNullOrWhiteSpace(signal.Value.Text) ? signal.Value.Name : signal.Value.Text;
        this.Editable = editable;
        _skill = signal.Value;
    }

    public void HandleTextChanged()
    {
        if (_skill == null) return;
        _skill.Text = this.Text;        
    }
}
