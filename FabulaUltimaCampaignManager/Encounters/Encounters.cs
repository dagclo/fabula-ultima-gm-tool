using FirstProject.Encounters;
using Godot;
using System;

public partial class Encounters : VBoxContainer
{

    [Signal]
    public delegate void UpdateCurrentEncounterEventHandler(Encounter encounter);

    [Signal]
    public delegate void AddEncounterEventHandler(Encounter encounter);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnAddEncounterButtonPressed()
    {
		var encounter = new Encounter()
        {
            Id = Guid.NewGuid().ToString(),
        };
        var wrappedEncounter = encounter;
        EmitSignal(SignalName.UpdateCurrentEncounter, wrappedEncounter);
        EmitSignal(SignalName.AddEncounter, wrappedEncounter);
    }

    public void OnLoadEncounter(Encounter encounter)
    {
        EmitSignal(SignalName.UpdateCurrentEncounter, encounter);
    }
}