using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class CurrentEncounter : PanelContainer
{
	private Encounter Encounter { get; set; }
	private RunState RunState { get; set; }

    [Signal]
    public delegate void UpdateEncounterEventHandler(Encounter encounter);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		// todo: show last run encounter
        if (Encounter == null)
        {
            this.Visible = false;
        }
		RunState = GetNode<RunState>("/root/RunState");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleUpdateEncounter(Encounter encounter)
	{
        Encounter = encounter;
		if(Encounter != null)
		{
			this.Visible = true;
            RunState.RunningEncounter = Encounter;
        }
		else
		{
			this.Visible = false;
		}
    }

	public void HideEncounterIfDeleted(Encounter encounter)
	{
		if (Encounter == null) return;
		if (encounter == null) return;
		var deletedEncounter = encounter;
		if (Encounter.Id != deletedEncounter.Id) return;
		this.Visible = false;
		Encounter = null;
	}

	public void AddNpcToEncounter(NpcInstance npc)
	{
        if (Encounter == null) return;
		if (npc == null) return;
		Encounter.AddNpc(npc); // use this to ensure change is emitted		
		EmitSignal(SignalName.UpdateEncounter, Encounter);
    }
}
