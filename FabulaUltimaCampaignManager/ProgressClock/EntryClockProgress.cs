using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

namespace FabulaUltimaGMTool.Model.ProgressClock;

public partial class EntryClockProgress : Label
{
    public void OnClockChanged(SignalWrapper<ProgressClockModel> wrapper)
    {
        var model = wrapper.Value;
        this.Text = $"{model.SectionStates.Count(s => s)}/{model.SectionStates.Count}";
    }
}
