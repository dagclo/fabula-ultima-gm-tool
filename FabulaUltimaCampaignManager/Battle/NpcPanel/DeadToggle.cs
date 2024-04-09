using FirstProject.Encounters;
using Godot;
using System;

public partial class DeadToggle : CheckButton, INpcStatusReader
{
    private BattleStatus _status;
    private int _lastHealth;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleStatusSet(BattleStatus status)
    {
        status.StatusChanged += OnStatusChanged;
        _status = status;
        OnStatusChanged(status);
    }

    private void OnStatusChanged(BattleStatus status)
    {
        if (_isPressed == status.IsDead) return;
        this.SetPressedNoSignal(status.IsDead);
    }

    private bool _isPressed = false;
    private void OnToggleDeath(bool on)
    {
        _isPressed = on;
        if (_status == null) return;
        if(on)
        {
            _lastHealth = _status.CurrentHP;
            _status.Kill();
        }
        else if(_status.IsDead)
        {
            _status.CurrentHP = _lastHealth;
            _lastHealth = 0;
        }        
    }
}
