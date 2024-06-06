using FirstProject;
using Godot;

public partial class Background : AnimatedSprite2D
{
    [Export]
    public AudioStreamPlayer AudioPlayer { get; set; }

    [Export]
    public Configuration Configuration { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var runState = GetNode<RunState>("/root/RunState");
        var targetAnimation = runState.RunningEncounter.Background.ToString();        
        this.Animation = targetAnimation;
        AudioPlayer.Playing = Configuration.BackgroundMusicEnabled;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
