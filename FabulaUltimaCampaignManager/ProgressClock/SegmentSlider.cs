using Godot;
using Godot.Collections;

public partial class SegmentSlider : HSlider
{
    public void HandleSectionStateUpdate(Array<bool> states)
    {
        this.Value = states.Count;
    }
}
