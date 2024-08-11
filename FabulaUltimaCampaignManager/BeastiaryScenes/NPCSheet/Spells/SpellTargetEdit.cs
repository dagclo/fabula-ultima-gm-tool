using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpellTargetEdit : LineEdit
{
    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        this.Text = $"{signal.Value.Target}";
    }
}
