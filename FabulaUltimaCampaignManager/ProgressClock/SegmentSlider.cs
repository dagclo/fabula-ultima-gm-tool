using Godot;
using Godot.Collections;
using System;

public partial class SegmentSlider : HSlider
{
    public void HandleSectionStateUpdate(Array<bool> states)
    {
        this.Value = states.Count;
    }
}
