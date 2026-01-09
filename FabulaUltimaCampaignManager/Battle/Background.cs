using FabulaUltimaGMTool;
using Godot;

public partial class Background : AnimatedSprite2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var runState = GetNode<RunState>("/root/RunState");
        var targetAnimation = runState.RunningEncounter.Background.ToString();        
        
        var userConfiguration = GetNode<UserConfigurationState>("/root/UserConfigurationState").UserConfigurationData;
        this.Animation = targetAnimation;
    }
}
