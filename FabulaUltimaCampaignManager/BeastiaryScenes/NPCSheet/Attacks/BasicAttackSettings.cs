using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class BasicAttackSettings : Container
{
    public BasicAttackTemplate BasicAttack { get; set; }

    [Signal]
    public delegate void AttackUpdateEventHandler(SignalWrapper<BasicAttackTemplate> signalWrapper);
    public Action<BasicAttackSettings> OnRemoveAttack { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (BasicAttack == null) return;
        EmitSignal(SignalName.AttackUpdate, new SignalWrapper<BasicAttackTemplate>(BasicAttack));
    }

    internal void HandleBeastUpdate()
    {
        if (BasicAttack == null) return;
        EmitSignal(SignalName.AttackUpdate, new SignalWrapper<BasicAttackTemplate>(BasicAttack));
    }

    public void HandleAttackRemove()
    {
        OnRemoveAttack?.Invoke(this);
    }
}
