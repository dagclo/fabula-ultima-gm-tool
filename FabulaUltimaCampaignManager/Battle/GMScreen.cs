using FirstProject.Encounters;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GMScreen : HSplitContainer, IEncounterReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadEncounter(Encounter encounter, IReadOnlyList<BattleStatus> battleStatuses)
    {
        if (encounter == null) return;

        var npcsPlusStatus = encounter.NpcCollection
            .Zip(battleStatuses, (c, s) => (c, s));            

        var npcInstanceReadersQueue = new Queue<INpcInstanceReader>
            (this.FindChildren("*")
            .Where(l => l is INpcInstanceReader)
            .Select(r => r as INpcInstanceReader)
            .Take(encounter.NpcCollection.Count));
       
        foreach ((var npc, var status) in npcsPlusStatus)
        {
            if(!npcInstanceReadersQueue.TryDequeue(out var reader)) 
            {
                throw new Exception("ran out of slots");
            }
            reader.ReadNpc(npc, status);
        }
    }
}
