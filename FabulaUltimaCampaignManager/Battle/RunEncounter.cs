using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class RunEncounter : Control
{
    private MessageRouter _messageRouter;
    private MessagePublisher<EncounterLog> _messagePublisher;

    [Export]
    public string NextScreenPath { get; set; } = "res://node_2d.tscn";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{		
        var runState = GetNode<RunState>("/root/RunState");
		var encounter = runState.RunningEncounter ?? throw new Exception("No encounter set");
        _messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = _messageRouter.GetPublisher<EncounterLog>();
        _messageRouter.RegisterSubscriber<EncounterEnd>(ExitEncounterAsync);

        var statuses = encounter.NpcCollection.Select((c, i) => new BattleStatus(c)).ToList();
        
        foreach (var child in this.FindChildren("*")
          .Where(l => l is IEncounterReader))
        {
            var reader = child as IEncounterReader;			
            reader.ReadEncounter(encounter, statuses);
        }

       
        CallDeferred(MethodName.DeferAction, encounter); // post initiative winner to log
    }

    private Task ExitEncounterAsync(IMessage message)
    {
        if (!(message is IMessage<EncounterEnd> encounterEnd)) return Task.CompletedTask;
        CallDeferred(MethodName.ExitEncounter);
        return Task.CompletedTask;
    }

    private void ExitEncounter()
    {
        GetTree().ChangeSceneToFile(NextScreenPath);
    }

    public void DeferAction(Encounter encounter)
    {
        var initiativeWinner = encounter.InitiativeSeed.PlayersWon ? "Players" : "Npcs";
        var log = new EncounterLog
        {
            Action = $"\n{initiativeWinner} Start",
            Id = Guid.NewGuid(),
            Verb = "1",
            Actor = "Round",
            DisplayLevel = DisplayLevel.WHOOSH
        };
        _messagePublisher.Publish(log.AsMessage());
    }

    public void HandleTreeExiting()
    {
        foreach(var player in GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid))
        {
            player.ActiveChanged = null;
        }
        _messageRouter.TearDown();
    }
}
