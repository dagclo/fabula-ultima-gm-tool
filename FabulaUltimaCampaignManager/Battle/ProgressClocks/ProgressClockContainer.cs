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

    private int _curXOffset = 0;
    private int _curYOffset = 0;

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

        // the generated escape clocks are battle infrastructure: hide-only
        foreach (var clockModel in _progressClockModels)
        {
            AddClockWindow(clockModel, startVisible: false, deleteDisabled: true);
        }

        // the scene's own clocks can be removed mid-battle
        var encounter = GetNode<RunState>("/root/RunState").RunningEncounter;
        encounter.ProgressClocks ??= new Array<ProgressClockModel>();
        foreach (var clockModel in encounter.ProgressClocks)
        {
            AddClockWindow(clockModel, startVisible: false, deleteDisabled: false);
        }

        ShowHide(false);
    }

    private ProgressClock AddClockWindow(ProgressClockModel clockModel, bool startVisible, bool deleteDisabled)
    {
        var progressClock = ProgressClockTemplate.Instantiate<ProgressClock>();
        progressClock.Visible = startVisible;
        progressClock.Model = clockModel;
        progressClock.OnHideClock += (ProgressClock c) => HandleHideClock(c);
        progressClock.DeleteDisabled = deleteDisabled;
        if (!deleteDisabled)
        {
            progressClock.OnRemove += (ProgressClockModel m) => HandleRemoveClock(progressClock, m);
        }
        progressClock.AddToGroup(ShowModeGroupName);
        progressClock.InitialPosition = Window.WindowInitialPosition.Absolute;
        progressClock.Position = progressClock.Position + new Vector2I(_curXOffset, _curYOffset);
        AddChild(progressClock);
        progressClock.Owner = this; // Owner must be set after AddChild: it has to be an ancestor in the tree
        // tile side by side (with a small stagger) instead of cascading 50px,
        // which buried each clock under the next one
        _curXOffset += progressClock.Size.X + XCascadeOffset;
        _curYOffset += YCascadeOffset;
        return progressClock;
    }

    private void HandleRemoveClock(ProgressClock clockWindow, ProgressClockModel model)
    {
        // sync the removal back to the scene (a no-op for a not-yet-saved clock);
        // it hits disk on the next campaign-screen save
        var encounter = GetNode<RunState>("/root/RunState").RunningEncounter;
        encounter.ProgressClocks?.Remove(model);
        RemoveChild(clockWindow);
        clockWindow.QueueFree();
        if (!GetChildren().OfType<ProgressClock>().Any(p => p.Visible))
        {
            ShowHide(false);
        }
    }

    private void HandleAddPressed()
    {
        // a null model makes the dialog create a fresh one and open in edit mode
        var progressClock = AddClockWindow(null, startVisible: true, deleteDisabled: false);
        var encounter = GetNode<RunState>("/root/RunState").RunningEncounter;
        progressClock.OnCommit += () =>
        {
            // attach the new clock to the scene on an explicit Save press
            // (written to disk on the next campaign-screen save)
            if (!encounter.ProgressClocks.Contains(progressClock.Model))
            {
                encounter.ProgressClocks.Add(progressClock.Model);
            }
        };
        ShowHide(true); // keep the toolbar state consistent with the now-visible clocks
    }

    private void HandleHideClock(ProgressClock c)
    {
        // the window X hides just this clock; the Show button re-shows all of them
        c.Visible = false;
        // if that was the last visible clock, flip the toolbar back to "Show"
        if (!GetChildren().OfType<ProgressClock>().Any(p => p.Visible))
        {
            ShowHide(false);
        }
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
