using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpecialAttackTextEdit : TextEdit
{
    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal)
    {
        this.Text = string.IsNullOrWhiteSpace(signal.Value.Text) ? signal.Value.Name : signal.Value.Text;
    }
}
