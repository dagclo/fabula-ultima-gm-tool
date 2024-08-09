using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AttackAccuracyModLabel : Label
{
    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        this.Text = $"Accuracy Mod: {attack.AccuracyMod}";
    }
}
