using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class FinishTurnButton : Button, INpcReader
{
    private NpcInstance _instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		this._instance = npc;
    }

	public void OnTurnChanged(SignalWrapper<ITurnOwner> signalWrapper)
	{
		if (this._instance == null) return;
		var turnOwner = signalWrapper.Value;
		if (turnOwner?.Match(_instance) != true) return;		
		this.Disabled = false;
	}

	public void OnButtonPressed()
	{
		this.Disabled = true;
	}
}
