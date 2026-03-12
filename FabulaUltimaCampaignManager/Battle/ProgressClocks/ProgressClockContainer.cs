using FabulaUltimaGMTool.Model.ProgressClock;
using FabulaUltimaGMTool.UI.ProgressClock;
using Godot;
using Godot.Collections;
using System;
using System.Linq;


public partial class ProgressClockContainer : VBoxContainer
{
    private Array<ProgressClockModel> _progressClockModels;

    [Export]
    public PackedScene ProgressClockTemplate { get; set; }

    [Export]
    public string HideModeGroupName { get; set; } = "hide_progress_clock";
    [Export]
    public string ShowModeGroupName { get; set; } = "show_progress_clock";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _progressClockModels = GetNode<RunState>("/root/RunState").RunningEncounter.ProgressClocks;
        _progressClockModels.Add(new ProgressClockModel
        {
            Title = "Players Escape (generated)",
            SectionStates = [false, false, false, false ]
        });
        _progressClockModels.Add(new ProgressClockModel
        {
            Title = "NPC Escape (generated)",
            SectionStates = [false, false, false, false]
        });

        foreach (var clockModel in _progressClockModels)
        {
            var progressClock = ProgressClockTemplate.Instantiate<ProgressClock>();
            progressClock.Model = clockModel;
            progressClock.OnHideClock += (ProgressClock c) => HandleHideClock(c);
            AddChild(progressClock);
            progressClock.Owner = this;
            progressClock.AddToGroup(ShowModeGroupName);            
        }

        ShowHide(false);
    }

    private void HandleHideClock(ProgressClock c)
    {
        c.Visible = false;
    }

    private void HandleHidePressed()
    {
        ShowHide(false);
    }

    private void ShowHide(bool showMode)
    {
        foreach (var control in GetTree().GetNodesInGroup(ShowModeGroupName).Select(n => n as Control))
        {
            control.Visible = showMode;
        }

        foreach (var control in GetTree().GetNodesInGroup(HideModeGroupName).Select(n => n as Control))
        {
            control.Visible = !showMode;
        }
    }

    private void HandleShowPressed()
    {
        ShowHide(true);
    }
}
