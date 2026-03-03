using Godot;
using System;
using System.Linq;

public partial class ProgressClock : Popup
{
	private Container _progressClock;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressClock = GetTree().GetNodesInGroup("progress_clock").Single() as Container;
    }

	public void SlotSelected(Control slot, int index)
	{

	}
}
