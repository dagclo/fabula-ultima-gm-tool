using FirstProject.Encounters;
using Godot;

public partial class StatusToggle : CheckButton, INpcStatusReader
{
	[Export]
	public StatusEffect Status { get; set; }

	private BattleStatus _battleStatus;

    public void HandleStatusSet(BattleStatus status)
    {
        _battleStatus = status;
    }

	public void OnToggle(bool toggleOn)
	{
		_battleStatus?.UpdateStatusEffect(Status, toggleOn);
	}
}
