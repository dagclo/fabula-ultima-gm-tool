using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddedActionEntry : VBoxContainer, IValidatable
{
    private BeastiaryRepository _beastRepository;
    private IBeastTemplate _beastTemplate;

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
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        if (Action == null) return;
        EmitSignal(SignalName.ActionSet, new SignalWrapper<ActionTemplate>(Action));
    }

    string IValidatable.Name => "Other Actions";

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (Action == null) return;
        _beastTemplate = beastTemplate;
        _beastRepository.QueueUpdates(_beastTemplate.Id, Action);
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveAction()
    {
        if (Action == null) return;
        _beastRepository.DequeueUpdate(_beastTemplate.Id, Action.Id);
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
