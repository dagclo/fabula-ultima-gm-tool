using FirstProject.Beastiary;
using Godot;
using System;

namespace FabulaUltimaGMTool.Model.ProgressClock;

public partial class ProgressClockEntry : HBoxContainer
{
    public ProgressClockModel ProgressClock { get; set; }

    [Signal]
    public delegate void ProgressClockChangedEventHandler(SignalWrapper<ProgressClockModel> wrapper);
    public Action<ProgressClockModel> OnShow { get; internal set; }
    public Action<ProgressClockModel> OnRemove { get; internal set; }

    private ConfirmationDialog _confirmDialog;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(MethodName.ModelChanged);
        _confirmDialog = new ConfirmationDialog { Title = "Remove Clock" };
        AddChild(_confirmDialog);
        _confirmDialog.Confirmed += () => OnRemove?.Invoke(ProgressClock);
    }

    public void ModelChanged()
    {
        EmitSignal(SignalName.ProgressClockChanged, new SignalWrapper<ProgressClockModel>(ProgressClock));
    }

    public void HandleShowButtonPressed()
    {
        OnShow?.Invoke(ProgressClock);
    }

    public void HandleRemoveButtonPressed()
    {
        var name = string.IsNullOrWhiteSpace(ProgressClock?.Title) ? "this clock" : $"\"{ProgressClock.Title}\"";
        _confirmDialog.DialogText = $"Remove {name}?";
        _confirmDialog.PopupCentered();
    }
}
