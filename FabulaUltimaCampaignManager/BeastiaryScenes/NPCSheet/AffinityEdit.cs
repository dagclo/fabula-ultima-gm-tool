using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class AffinityEdit : Control, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Signal]
    public delegate void UpdateAffinityEventHandler(string affinityValue);

    [Signal]
    public delegate void UpdateElementEventHandler(string element);

    [Export]
    public string AffinityName { get; set; }

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(AffinityName != null) EmitSignal(SignalName.UpdateElement, AffinityName);
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;
        EmitSignal(SignalName.UpdateAffinity, _beastTemplate.Resistances.TryGetValue(AffinityName.ToLowerInvariant(), out var resistance) ? resistance.Affinity : string.Empty);
    }
}
