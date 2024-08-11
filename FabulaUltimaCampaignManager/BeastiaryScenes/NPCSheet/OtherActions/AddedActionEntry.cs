using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddedActionEntry : VBoxContainer, IValidatable
{
    [Signal]
    public delegate void ActionSetEventHandler(SignalWrapper<ActionTemplate> action);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> beast);

    public ActionTemplate Action { get; internal set; }
    public Action OnUpdateBeast { get; internal set; }
    public Action<AddedActionEntry> OnRemoveAction { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Action == null) return;
        EmitSignal(SignalName.ActionSet, new SignalWrapper<ActionTemplate>(Action));
    }

    string IValidatable.Name => "Other Actions";

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (Action == null) return;
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveAction()
    {
        OnRemoveAction?.Invoke(this);
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }

    public IEnumerable<TemplateValidation> Validate()
    {
        if (Action == null) yield break;
        if (string.IsNullOrWhiteSpace(Action.Name)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Action missing Name" };
        if (string.IsNullOrWhiteSpace(Action.Effect)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Action '{Action.Name}' missing description" };
    }
}
