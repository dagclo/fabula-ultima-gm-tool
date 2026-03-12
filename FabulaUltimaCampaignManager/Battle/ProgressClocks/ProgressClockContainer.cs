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

    [Export]
    public int XCascadeOffset { get; set; } = 50;

    [Export]
    public int YCascadeOffset { get; set; } = 50;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _progressClockModels = new Array<ProgressClockModel>();
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

        _progressClockModels.AddRange(GetNode<RunState>("/root/RunState").RunningEncounter.ProgressClocks);

        var curYOffSet = 0;
        var curXOffSet = 0;

        foreach (var clockModel in _progressClockModels)
        {
            var progressClock = ProgressClockTemplate.Instantiate<ProgressClock>();
            progressClock.Visible = false;
            progressClock.Model = clockModel;
            progressClock.OnHideClock += (ProgressClock c) => HandleHideClock(c);
            progressClock.Owner = this;
            progressClock.DeleteDisabled = true;
            progressClock.AddToGroup(ShowModeGroupName);
            progressClock.InitialPosition = Window.WindowInitialPosition.Absolute;
            progressClock.Position = progressClock.Position + new Vector2I(curXOffSet, curYOffSet);            
            AddChild(progressClock);
            curYOffSet += YCascadeOffset;
            curXOffSet += XCascadeOffset;
        }

        ShowHide(false);
    }

    private void HandleHideClock(ProgressClock c)
    {
        // do nothing for now
    }

    private void HandleHidePressed()
    {
        ShowHide(false);
    }

    private void ShowHide(bool showMode)
    {
        void SetGroupVisibility(Node node, bool visible)
        {
            if (node is Control control)
            {
                control.Visible = visible;
            }
            else if (node is Window window)
            {
                window.Visible = visible;
            }
        }

        foreach (var node in GetTree().GetNodesInGroup(ShowModeGroupName))
        {
            SetGroupVisibility(node, showMode);
        }

        foreach (var node in GetTree().GetNodesInGroup(HideModeGroupName))
        {            
            SetGroupVisibility(node, !showMode);
        }
    }

    private void HandleShowPressed()
    {
        ShowHide(true);
    }
}
