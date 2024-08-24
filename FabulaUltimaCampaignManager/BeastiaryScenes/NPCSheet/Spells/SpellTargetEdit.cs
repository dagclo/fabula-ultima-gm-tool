using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellTargetEdit : LineEdit
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        this.Text = $"{signal.Value.Target}";
        _spell = signal.Value;
        this.Editable = editable;
    }

    public void HandleTextChanged(string newText)
    {
        _spell.Target = newText;
    }
}
