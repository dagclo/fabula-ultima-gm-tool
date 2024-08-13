using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellTextEdit : TextEdit
{
    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        this.Text = $"{signal.Value.Description}";
    }
}
