using FabulaUltimaGMTool.Model.ProgressClock;
using FirstProject.Beastiary;
using Godot;
using System;

namespace FabulaUltimaGMTool.Model.ProgressClock;

public partial class EntryClockName : Label
{
	public void OnClockChanged(SignalWrapper<ProgressClockModel> wrapper)
	{
		var model = wrapper.Value;
		this.Text = model.Title;
	}
}
