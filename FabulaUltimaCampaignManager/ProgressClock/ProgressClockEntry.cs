using FabulaUltimaGMTool.Model.ProgressClock;
using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

namespace FabulaUltimaGMTool.Model.ProgressClock;

public partial class ProgressClockEntry : HBoxContainer
{
    public ProgressClockModel ProgressClock { get; set; }

    [Signal]
    public delegate void ProgressClockChangedEventHandler(SignalWrapper<ProgressClockModel> wrapper);
    public Action<ProgressClockModel> OnShow { get; internal set; }    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(MethodName.ModelChanged);
    }

    public void ModelChanged()
    {
        EmitSignal(SignalName.ProgressClockChanged, new SignalWrapper<ProgressClockModel>(ProgressClock));
    }

    public void HandleShowButtonPressed()
    {
        OnShow?.Invoke(ProgressClock);
    }
}
