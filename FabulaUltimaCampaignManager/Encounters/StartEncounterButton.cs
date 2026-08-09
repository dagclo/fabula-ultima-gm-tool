using FirstProject.Encounters;
using Godot;
using System;

public partial class StartEncounterButton : Button
{
	[Export]
	public PackedScene RunEncounterScene { get; set; }
    public Action<PackedScene, Encounter> OnStartEncounter { get; private set; }

    private AcceptDialog _errorDialog;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var popup = this.GetChild<InitiativePopup>(0);
		this.OnStartEncounter += popup.OnStartEncounter;
        _errorDialog = new AcceptDialog { Title = "Can't Start Scene" };
        AddChild(_errorDialog);
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
			var reasons = new System.Collections.Generic.List<string>();
			if (!runState.HasNpcs) reasons.Add("• Add at least one NPC to the scene");
			if (!runState.HasPlayers) reasons.Add("• Name and enable at least one player");
			_errorDialog.DialogText = string.Join("\n", reasons);
			_errorDialog.PopupCentered();
			return;
		}

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
