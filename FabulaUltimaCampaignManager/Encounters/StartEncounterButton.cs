using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;

public partial class StartEncounterButton : Button
{
	[Export]
	public PackedScene RunEncounterScene { get; set; }
    public Action<PackedScene, Encounter> OnStartEncounter { get; private set; }    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var popup = this.GetChild<InitiativePopup>(0);
		this.OnStartEncounter += popup.OnStartEncounter;		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnButtonPressed()
	{	
        var runState = GetNode<RunState>("/root/RunState");
		if (!runState.IsValid)
		{
			GD.Print("not valid");
			// error popup
			return;
		}
		
		var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
		messageRouter.TearDown();

        this.OnStartEncounter?.Invoke(RunEncounterScene, runState.RunningEncounter);        
    }

	private void ChangeScene()
	{
        GetTree().ChangeSceneToPacked(RunEncounterScene);
	}

	public void OnSwitchScene()
	{
		// deferring call because changing scenes has to happen on main thread        
		this.CallDeferred(StartEncounterButton.MethodName.ChangeScene);		
    }
}
