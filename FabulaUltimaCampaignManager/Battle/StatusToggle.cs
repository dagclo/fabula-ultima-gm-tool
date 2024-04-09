using FirstProject.Encounters;
using Godot;

public partial class StatusToggle : CheckButton, INpcStatusReader
{
	[Export]
	public StatusEffect Status { get; set; }

	private BattleStatus _battleStatus;
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
        _battleStatus = status;
    }

	public void OnToggle(bool toggleOn)
	{
		_battleStatus?.UpdateStatusEffect(Status, toggleOn);
	}
}
