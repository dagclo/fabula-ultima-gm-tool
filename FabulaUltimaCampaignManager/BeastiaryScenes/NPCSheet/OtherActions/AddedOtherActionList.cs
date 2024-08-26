using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AddedOtherActionList : VBoxContainer, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public PackedScene AddActionScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
    public Action<IBeastTemplate> OnBeastChanged { get; private set; }

    private bool _addedExisting = false;
    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        OnBeastChanged?.Invoke(beastTemplate);
        if (_addedExisting) return;
        foreach(var action in _beastTemplate.Model.Actions)
        {
            AddOtherAction(action);
        }
        _addedExisting = true;
    }

    public void HandleAddAction()
    {
        var action = new ActionTemplate()
        {
            Id = Guid.NewGuid()
        };
        _beastTemplate.Model.AddAction(action);
        AddOtherAction(action);
        BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
    }

    private void AddOtherAction(ActionTemplate action)
    {
        var scene = AddActionScene.Instantiate<AddedActionEntry>();
        scene.Action = action;
        scene.OnRemoveAction += HandleRemoveAction;
        scene.OnUpdateBeast += HandleUpdateBeast;
        OnBeastChanged += scene.HandleBeastChanged;
        AddChild(scene);
        scene.HandleBeastChanged(_beastTemplate);
    }

    private void HandleRemoveAction(AddedActionEntry entry)
    {
        _beastTemplate.Model.RemoveAction(entry.Action);
        OnBeastChanged -= entry.HandleBeastChanged;
        RemoveChild(entry);
        entry.QueueFree();
        OnBeastChanged?.Invoke(_beastTemplate);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.CHANGED }));
    }

    private void HandleUpdateBeast()
    {
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.CHANGED }));
    }
}
