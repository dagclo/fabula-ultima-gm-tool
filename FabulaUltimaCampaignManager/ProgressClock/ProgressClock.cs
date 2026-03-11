using FabulaUltimaGMTool.Model.ProgressClock;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FabulaUltimaGMTool.UI.ProgressClock;

public partial class ProgressClock : Window
{
	private Control _progressClock;
	private ICollection<Control> _viewNodes;
	private ICollection<Control> _editNodes;
    public ProgressClockModel Model { get; set; }
    [Export]
    public string ViewModeGroupName { get; set; } = "view_mode";
    [Export]
    public string EditModeGroupName { get; set; } = "edit_mode";
    [Export]
    public string ClockSectionGroupName { get; set; } = "clock_sections";

    [Export]
    public string ProgressClockGroupName { get; set; } = "progress_clock";
    public Action<ProgressClockModel> OnRemove { get; internal set; }
    public Action<ProgressClock> OnHideClock { get; internal set; }
    public Action OnSave { get; internal set; }

    [Signal]
    public delegate void ClockTitleUpdateEventHandler(string newTitle);

    [Signal]
    public delegate void UpdateSectionStatesEventHandler(Array<bool> states);

    [Signal]
    public delegate void SetClockTitleEventHandler(string title);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_progressClock = FindChildren("*").Single(c => c.IsInGroup(ProgressClockGroupName)) as Control;
		_viewNodes = FindChildren("*").Where(c => c.IsInGroup(ViewModeGroupName)).Select(n => n as Control).ToList();
        _editNodes = FindChildren("*").Where(c => c.IsInGroup(EditModeGroupName)).Select(n => n as Control).ToList();        
        
        if (Model == null)
        {
            Model = new ProgressClockModel();
            var sectionStates = FindChildren("*").Where(c => c.IsInGroup(ClockSectionGroupName)).Select(s => false);
            Model.PushStates(sectionStates);
            SetMode(false);
        }
        else
        {
            foreach(var control in FindChildren("*").Where(c => c.IsInGroup(ClockSectionGroupName)).Select(n => n as Control))
            {
                if(control.GetParent() != _progressClock)
                {
                    GD.PrintErr($"{control.Name} isn't parent of {_progressClock.Name}");
                    continue;
                }
                _progressClock.RemoveChild(control);
                control.QueueFree();
            }

            foreach(var section in Enumerable.Range(0, Model.SectionStates.Count()).Select(i => CreateSection(i)))
            {                
                _progressClock.AddChild(section);
                section.Owner = _progressClock;
                section.AddToGroup(ClockSectionGroupName);
            }
            CallDeferred(MethodName.SetSectionStates);
            CallDeferred(MethodName.ClockUpdated);
            EmitSignal(SignalName.SetClockTitle, Model.Title);
            SetMode(true);
        }
        
        Model.Changed += ClockUpdated;
        
    }

    private void SetSectionStates()
    {
        EmitSignal(SignalName.UpdateSectionStates, Model.SectionStates);
    }

    private void ClockUpdated()
    {
        EmitSignal(SignalName.ClockTitleUpdate, Model.Title);
        
        int numSections = FindChildren("*").Where(c => c.IsInGroup(ClockSectionGroupName)).Count();
        if (numSections < Model.SectionStates.Count())
        {
            foreach (var section in Enumerable.Range(Model.SectionStates.Count - 1, Model.SectionStates.Count - numSections).Select(i => CreateSection(i)))
            {
                _progressClock.AddChild(section);
                section.Owner = _progressClock;
                section.AddToGroup(ClockSectionGroupName);
            }
        }
        else if (numSections > Model.SectionStates.Count)
        {
            foreach(var section in FindChildren("*").Where(c => c.IsInGroup(ClockSectionGroupName)).Skip(Model.SectionStates.Count))
            {
                _progressClock.RemoveChild(section);
                section.QueueFree();
            }
        }
        OnSave?.Invoke();
    }

    private ColorRect CreateSection(int i)
    {
        var result = new ColorRect();
        result.Color = new Color("ffffff00");
        result.Name = $"Section_{i}";        
        return result;
    }

	public void SlotSelected(Control slot, int _, bool state)
	{
        var index = int.Parse(slot.Name.ToString().Split('_').Last());
        Model.SectionStates[index] = state;
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
        if (Model.SectionStates.Count == sections) return;
        if (Model.SectionStates.Count > sections)
        {
            Model.ReduceStates(sections);
        }
        else if(Model.SectionStates.Count < sections)
        {
            Model.PushStates(Enumerable.Range(0, sections - Model.SectionStates.Count).Select(_ => false));
            
        }
        CallDeferred(MethodName.SetSectionStates);
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

    private void Handle_CloseRequested()
    {
        OnHideClock?.Invoke(this);
    }

    private void Handle_RemovePressed()
    {
        OnRemove?.Invoke(this.Model);
    }
}
