using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class FinishTurnButton : Button, INpcReader, INpcStatusReader
{
    private NpcInstance _instance;
    private BattleStatus _status;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		this._instance = npc;
    }

	public void OnRoundChanged()
	{
		if (this._instance == null) return;
        if (this._status == null) return;
        this.Disabled = false;
        _status.IsActive = true;
    }

	public void OnButtonPressed() 
	{
		this.Disabled = true;
		_status.IsActive = false;
	}

    public void HandleStatusSet(BattleStatus status)
    {        
        _status = status;        
    }
}
