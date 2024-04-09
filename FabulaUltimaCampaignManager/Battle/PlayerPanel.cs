using FirstProject.Campaign;
using Godot;
using System;

public partial class PlayerPanel : PanelContainer, IPlayerTurnHandler
{
	private PlayerData _playerData;
    private CallDeferrer _callDeferrer;

    public Action CompletedTurn { get; set; }

    [Signal]
    public delegate void PlayerUpdatedEventHandler(PlayerData player);

    [Signal]
    public delegate void RoundStartEventHandler();

    [Signal]
    public delegate void RoundEndEventHandler();


    [Signal]
    public delegate void TurnStartEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Visible = false;
        _callDeferrer = new CallDeferrer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        _callDeferrer.RunNext()?.Invoke();
    }

    public void ReadPlayer(PlayerData player)
    {
        _playerData = player;
        _playerData.IsActive = true;
        this.Visible = true;
		EmitSignal(SignalName.PlayerUpdated, player);
    }

    public void OnRoundStart()
    {
        _callDeferrer.Defer(() => EmitSignal(SignalName.RoundStart));
        _playerData.IsActive = true;
    }

    public void OnCompletedTurn()
    {
        CompletedTurn?.Invoke();
        _playerData.IsActive = false;
    }

    public void OnTurnStart()
    {
        _callDeferrer.Defer(() => EmitSignal(SignalName.TurnStart));
    }

    public void OnRoundEnd()
    {
        EmitSignal(SignalName.RoundEnd);
    }
}
