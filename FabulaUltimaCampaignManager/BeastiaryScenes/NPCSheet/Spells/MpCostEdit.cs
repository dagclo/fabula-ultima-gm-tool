using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class MpCostEdit : LineEdit
{
    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        this.Text = $"{signal.Value.MagicPointCost}";
    }
}
