using FabulaUltimaGMTool.Model.ProgressClock;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FabulaUltimaGMTool.UI.ProgressClock;

public partial class ProgressClock : Popup
{
	private Control _progressClock;
	private ICollection<Control> _viewNodes;
	private ICollection<Control> _editNodes;
    public ProgressClockModel Model { get; set; }

    [Signal]
    delegate void ClockTitleUpdateEventHandler(string newTitle);
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_progressClock = GetTree().GetNodesInGroup("progress_clock").Single() as Control;
		_viewNodes = GetTree().GetNodesInGroup("view_mode").Select(n => n as Control).ToList();
        _editNodes = GetTree().GetNodesInGroup("edit_mode").Select(n => n as Control).ToList();
        SetMode(false);
        if(Model == null) Model = new ProgressClockModel { Sections = 4 };

        Model.Changed += ClockUpdated;

        foreach(var child in _progressClock.FindChildren("*"))
        {
            _progressClock.RemoveChild(child);
            child.QueueFree();
        }

        foreach(var num in Enumerable.Range(0, Model.Sections))
        {
            var section = CreateSection();
            _progressClock.AddChild(section);
        }
    }

    private void ClockUpdated()
    {
        EmitSignal(SignalName.ClockTitleUpdate, Model.Title);
        
    }

    private ColorRect CreateSection()
    {
        var result = new ColorRect();
        result.Color = new Color("ffffff00");
        return result;
    }

	public void SlotSelected(Control slot, int index)
	{

	}

    public void ClockTitleChanged (string newText)
    {
        Model.Title = newText;        
    }

	public void SaveButtonPressed()
	{
        SetMode(true);
    }

    public void EditButtonPressed()
    {
		SetMode(false);
    }

    public void SectionsChanged(float sectionCount)
    {
        var sections = (int)sectionCount;
        if(Model.Sections != sections) Model.Sections = sections;
    }

	private void SetMode(bool isViewMode)
	{
        foreach (var node in _viewNodes)
        {
            node.Visible = isViewMode;
        }

        foreach (var node in _editNodes)
        {
            node.Visible = !isViewMode;
        }
    }
}
