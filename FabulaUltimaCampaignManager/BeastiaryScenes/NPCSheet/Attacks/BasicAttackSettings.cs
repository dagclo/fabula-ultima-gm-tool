using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class BasicAttackSettings : Container, IValidatable
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

    string IValidatable.Name => "Basic Attack";
    public IEnumerable<TemplateValidation> Validate()
    {
        if (string.IsNullOrWhiteSpace(BasicAttack.Name)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Name Not Set" };
        if (string.IsNullOrWhiteSpace(BasicAttack.Attribute1)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Attribute1 Not Set" };
        if (string.IsNullOrWhiteSpace(BasicAttack.Attribute2)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Attribute2 Not Set" };
        if (BasicAttack.DamageType == null) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Damage Type not set" };
    }
}
