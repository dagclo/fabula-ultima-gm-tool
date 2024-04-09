using Godot;

public partial class Background : AnimatedSprite2D
{
    private AnimationPlayer _animationPlayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var runState = GetNode<RunState>("/root/RunState");
        var targetAnimation = runState.RunningEncounter.Background.ToString();        
        this.Animation = targetAnimation;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
