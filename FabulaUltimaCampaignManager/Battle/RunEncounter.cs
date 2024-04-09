using Godot;
using System;
using System.Linq;

public partial class RunEncounter : Control
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
        var runState = GetNode<RunState>("/root/RunState");
		var encounter = runState.RunningEncounter ?? throw new Exception("No encounter set");
        var statuses = encounter.NpcCollection.Select((c, i) => new BattleStatus(c)).ToList();
        
        foreach (var child in this.FindChildren("*")
          .Where(l => l is IEncounterReader))
        {
            var reader = child as IEncounterReader;			
            reader.ReadEncounter(encounter, statuses);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
