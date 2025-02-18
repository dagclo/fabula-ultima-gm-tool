using Godot;

public partial class VolumeSlider : HSlider
{
    private RunState _runState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _runState = GetNode<RunState>("/root/RunState");
    }

    public void HandleDragEnded(bool valueChanged)
	{
		if (!valueChanged) return;
        _runState.VolumeLevel = this.Value;
    }
}
