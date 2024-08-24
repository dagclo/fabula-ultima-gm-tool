using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellDurationEdit : LineEdit
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        this.Text = $"{signal.Value.Duration}";
        this.Editable = editable;
        _spell = signal.Value;
    }

    public void HandleTextChanged(string newText)
    {
        _spell.Duration = this.Text;
    }
}
