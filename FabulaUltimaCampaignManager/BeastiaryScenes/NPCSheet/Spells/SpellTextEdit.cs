using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellTextEdit : TextEdit
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        this.Text = $"{signal.Value.Description}";
        _spell = signal.Value;
        this.Editable = editable;
    }

    public void OnTextChanged()
    {
        _spell.Description = this.Text;
    }
}
