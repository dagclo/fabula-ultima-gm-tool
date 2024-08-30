using Godot;

public partial class LoadingBeastAnimation : PanelContainer
{
    private AnimationPlayer _animationPlayer;

    [Export]
    public string SpinAnimation { get; set; } = "rotate";

    [Export]
    public string ResetAnimation { get; set; } = "RESET";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Visible = false;
        _animationPlayer = GetNode<AnimationPlayer>("LoadingAnimationPlayer");
    }

	public void HandleLoading(bool loading)
	{
		this.Visible = loading;
        if (loading)
        {
            _animationPlayer.Play(SpinAnimation);
            return;
        }
        _animationPlayer.Play(ResetAnimation);
    }
}
