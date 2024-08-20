using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class FinishTurnButton : Button, INpcReader, INpcStatusReader
{
    private NpcInstance _instance;
    private BattleStatus _status;

    public void HandleNpcChanged(NpcInstance npc)
    {
		this._instance = npc;
    }

	public void OnRoundChanged()
	{
		if (this._instance == null) return;
        if (this._status == null) return;
        this.Disabled = false;
        _status.ResetTurns();
    }

	public void OnButtonPressed() 
	{
        _status.DecrementTurns();
        this.Disabled = _status.IsActive;		
	}

    public void HandleStatusSet(BattleStatus status)
    {        
        _status = status;        
    }
}
