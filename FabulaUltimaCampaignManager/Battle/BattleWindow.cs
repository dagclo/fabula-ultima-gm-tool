using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleWindow : Window, IEncounterReader
{
    private MessagePublisher<EncounterEnd> _endPublisher;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ResizeForResolution();
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _endPublisher = messageRouter.GetPublisher<EncounterEnd>();
        // closing the player-facing window ends the scene, same as the GM's End Encounter button
        this.CloseRequested += () => _endPublisher.Publish(new EncounterEnd().AsMessage());
    }

    public void ReadEncounter(Encounter encounter, IReadOnlyList<BattleStatus> battleStatuses)
    {   
        if (encounter == null) return;
        this.Visible = true;

        var npcsPlusStatus = encounter.NpcCollection
           .Zip(battleStatuses, (c, s) => (c, s));

        var npcInstanceReadersQueue = new Queue<INpcInstanceReader>
            (this.FindChildren("*")
            .Where(l => l is INpcInstanceReader)
            .Select(r => r as INpcInstanceReader)
            .Take(encounter.NpcCollection.Count));

        bool takenFirst = false;
        foreach ((var npc, var status) in npcsPlusStatus)
        {
            if (!npcInstanceReadersQueue.TryDequeue(out var reader))
            {
                throw new Exception("ran out of slots");
            }
            if (!takenFirst)
            {
                //status.IsActive = true; // don't remember why this is here
                takenFirst = true;
            }
            reader.ReadNpc(npc, status);
        }
    }
}
