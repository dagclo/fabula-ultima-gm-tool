using FabulaUltimaNpc;
using Godot;

public partial class OtherActionName : Label, IActionReceiver
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleSkillChanged(ActionTemplate action)
    {
        this.Text = action.Name;
    }
}
