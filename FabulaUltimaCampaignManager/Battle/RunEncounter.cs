using FirstProject.Encounters;
using FirstProject.Messaging;
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

       
        CallDeferred(MethodName.DeferAction, encounter); // post initiative winner to log
    }

    public void DeferAction(Encounter encounter)
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        var messagePublisher = messageRouter.GetPublisher<EncounterLog>();
        var log = new EncounterLog
        {
            Action = "Initiative",
            Id = Guid.NewGuid(),
            Verb = "won",
            Actor = encounter.InitiativeSeed.PlayersWon ? "Players" : "Npcs"
        };
        messagePublisher.Publish(log.AsMessage());
    }
}
