using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellDurationEdit : LineEdit
{
    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        this.Text = $"{signal.Value.Duration}";
    }
}
