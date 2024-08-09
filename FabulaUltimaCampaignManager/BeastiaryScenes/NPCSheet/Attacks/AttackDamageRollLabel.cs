using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AttackDamageRollLabel : Label
{
    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        this.Text = $"Damage Roll: HR + {attack.DamageMod}";
    }
}
