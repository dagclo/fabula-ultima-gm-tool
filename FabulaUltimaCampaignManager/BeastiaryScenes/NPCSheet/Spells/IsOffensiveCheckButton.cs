using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class IsOffensiveCheckButton : CheckButton
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        _spell = signal.Value;
        this.SetPressedNoSignal(_spell.IsOffensive);
        this.Disabled = !editable;
    }

    public void HandleToggled(bool on)
    {
        _spell.IsOffensive = on;
    }
}
