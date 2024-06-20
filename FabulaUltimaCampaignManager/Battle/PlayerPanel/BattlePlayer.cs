using FirstProject.Campaign;
using Godot;

public partial class BattlePlayer : HBoxContainer, IPlayerStatus
{
    private PlayerData _player;

    [Signal]
    public delegate void PlayerUpdatedEventHandler(PlayerData player);

    // Called when the node enters the scene tree for the first time.s
    public override void _Ready()
	{
		this.Visible = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadPlayer(PlayerData player)
    {
        _player = player;
        _player.ActiveChanged += this.OnActiveChanged;
        this.Visible = true;
        EmitSignal(SignalName.PlayerUpdated, player);
    }

    private void OnActiveChanged(bool isActive)
    {
        CallDeferred(MethodName.EmitSignal, SignalName.PlayerUpdated, _player);
    }
}
