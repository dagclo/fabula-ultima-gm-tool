using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class IsOffensiveCheckButton : CheckButton
{
    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        var spell = signal.Value;
        this.SetPressedNoSignal(spell.IsOffensive);
    }
}