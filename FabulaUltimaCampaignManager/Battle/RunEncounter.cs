using FabulaUltimaGMTool.Battle;
using FirstProject;
using FirstProject.Encounters;
using FirstProject.Messaging;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class RunEncounter : Control
{
    private MessageRouter _messageRouter;    

    [Export]
    public string NextScreenPath { get; set; } = "res://node_2d.tscn";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{		
        var runState = GetNode<RunState>("/root/RunState");
		var encounter = runState.RunningEncounter ?? throw new Exception("No encounter set");
        _messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        
        _messageRouter.RegisterSubscriber<EncounterEnd>(ExitEncounterAsync);

        var statuses = new Godot.Collections.Array<BattleStatus>(encounter.NpcCollection.Select((c) => new BattleStatus(c)));
        
        foreach (var child in this.FindChildren("*")
          .Where(l => l is IEncounterReader))
        {
            var reader = child as IEncounterReader;			
            reader.ReadEncounter(encounter, statuses);
        }

       
        CallDeferred(MethodName.InitializeEncounter, encounter, statuses); // post initiative winner to log
    }

    private Task ExitEncounterAsync(IMessage message)
    {
        if (!(message is IMessage<EncounterEnd> _)) return Task.CompletedTask;
        CallDeferred(MethodName.ExitEncounter);
        return Task.CompletedTask;
    }

    private void ExitEncounter()
    {
        GetTree().ChangeSceneToFile(NextScreenPath);
    }

    public void InitializeEncounter(Encounter encounter, Godot.Collections.Array<BattleStatus> statuses)
    {
        var initiativeWinner = encounter.InitiativeSeed.PlayersWon ? "Players" : "Npcs";
        var log = new EncounterLog
        {
            Action = $"{initiativeWinner} Start",
            Id = Guid.NewGuid(),
            Verb = "1",
            Actor = "Round",
            DisplayLevel = DisplayLevel.WHOOSH
        };
        var logMessagePublisher = _messageRouter.GetPublisher<EncounterLog>();
        logMessagePublisher.Publish(log.AsMessage());

        var initMessagePublisher = _messageRouter.GetPublisher<EncounterInitialize>();
        var initial = new EncounterInitialize
        {
            Npcs = statuses.Select(s => (encounter.NpcCollection.Single(n => n.Id.Equals(s.ToString())), s)).ToArray()
        };
        initMessagePublisher.Publish(initial.AsMessage());
    }

    public void HandleTreeExiting()
    {
        var runState = GetNode<RunState>("/root/RunState");
        foreach(var player in runState.Campaign.Players.Where(p => p.IsValid))
        {
            player.ActiveChanged = null;
        }

        // persist anything the battle changed on the campaign (clock fills,
        // mid-battle clock adds/removes) — this also covers quitting the app
        // from inside the battle, where no campaign-screen save can happen
        var configuration = GD.Load<FirstProject.Configuration>("res://configuration.tres");
        if (runState.Campaign != null && configuration != null)
        {
            runState.Campaign.Save(configuration.CampaignFolder + $"{runState.Campaign.Id}.tres");
        }

        var _ = _messageRouter.TearDown();
    }
}
