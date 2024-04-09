using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class TurnTracker : GridContainer, IEncounterReader
{
	private RoundState _roundState = new RoundState
	{
		RoundNumber = 1,
		CurrentTurnOwner = null,
        TurnNumber = 1,
	};
    private MessagePublisher<RoundState> _messagePublisher;
    
    private RoundRing<ITurnOwner> _turnOrder;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<TurnDoneMessage>(this.ReceiveTurnDoneMessage);
        _messagePublisher = messageRouter.GetPublisher<RoundState>();       
    }

    private Task ReceiveTurnDoneMessage(IMessage message)
    {
        if(!(message is IMessage<TurnDoneMessage> turnDoneMessage))
        {
            throw new Exception("wrong message type");
        }

        if (turnDoneMessage.Value.TurnOwner != _roundState.CurrentTurnOwner) return Task.CompletedTask; // ignore messages from wrong owner

        var roundState = GetNextRoundState(_roundState);
        _roundState = roundState;
        _messagePublisher.Publish(_roundState.AsMessage());
        return Task.CompletedTask;
    }

    private RoundState GetNextRoundState(RoundState roundState)
    {
        
        _turnOrder.MoveNext();
        var turnOwner = _turnOrder.GetCurrent();
        
        int turnNumber;
        int roundNumber;
        if (_turnOrder.AtStart())
        {
            turnNumber = 1;
            roundNumber = roundState.RoundNumber + 1;
        }
        else
        {
            turnNumber = roundState.TurnNumber + 1;
            roundNumber = roundState.RoundNumber;
        }

        return new RoundState
        {
            TurnNumber = turnNumber,
            CurrentTurnOwner = turnOwner,
            RoundNumber = roundNumber,
        };
    }

    public void ReadEncounter(Encounter encounter, IReadOnlyList<BattleStatus> battleStatuses)
    {
        var npcs = encounter.NpcCollection
            .Zip(battleStatuses, (c, s) => (c , s))
            .OrderByDescending(p => p.c.Template.Initiative)
            .SelectMany(p => Enumerable.Range(0, p.c.Model.Rank.GetNumSoldiersReplaced()).Select(_ => new NpcTurnOwner(p.c, p.s)))
            .ToArray();

        var playerQueue = new Queue<ITurnOwner>(Enumerable.Range(0, encounter.InitiativeSeed.NumPlayers).Select(_ => new PlayerTurnOwner(new BattleStatus())));
        var npcQueue = new Queue<ITurnOwner>(npcs);

        ITurnOwner start;
        if (encounter.InitiativeSeed.PlayersWon)
		{	
			_roundState.CurrentTurnOwner = playerQueue.Peek();
            start = playerQueue.Dequeue();
        }
		else
		{
			_roundState.CurrentTurnOwner = npcQueue.Peek();
            start = npcQueue.Dequeue();
        }
        
        IEnumerable<ITurnOwner> GenerateTurnOrder(
            ITurnOwner start, 
            Queue<ITurnOwner> playerOwners, 
            Queue<ITurnOwner> npcOwners)
        {
            
            yield return start;
            var last = start;
            while (playerOwners.Any() || npcOwners.Any())
            {
                ITurnOwner next;
                if (last.TurnOwnerKind == TurnOwnerKind.NPC && playerOwners.Any())
                {
                    next = playerOwners.Dequeue();   
                }
                else if(last.TurnOwnerKind == TurnOwnerKind.PLAYER && npcOwners.Any())
                {
                    next = npcOwners.Dequeue();
                }
                else
                {
                    next = playerOwners.Any() ? playerOwners.Dequeue() : npcOwners.Dequeue();
                }
                yield return next;
                last = next;
            }
        }
        var ordering = GenerateTurnOrder(start, playerQueue, npcQueue).ToArray();
        _turnOrder = new RoundRing<ITurnOwner>(ordering);
        _messagePublisher.Publish(_roundState.AsMessage());
    }

    public class RoundRing<T>
    {
        private RingMember<T> _current;
        public RoundRing(IEnumerable<T> items)
        {
            var members = items.Select(i => new RingMember<T> {  Item = i }).ToArray();
            if (!members.Any()) throw new ArgumentException(nameof(items));
            var start = members.First();
            start.IsStart = true;
            RingMember<T> last = start;
            foreach(var member in members.Skip(1))
            { 
                last.Next = member;
                last = member;
            }
            last.Next = start;
            _current = start;
        }

        public void MoveNext() => _current = _current.Next;

        public T GetCurrent() => _current.Item;

        public bool AtStart() => _current.IsStart;

        private class RingMember<TMemberType>
        {
            public TMemberType Item { get; set; }
            public bool IsStart { get; set; } = false;
            public RingMember<T> Next { get; set; }
        }
    }

    public class PlayerTurnOwner : ITurnOwner
    {
        public PlayerTurnOwner(BattleStatus status)
        {
            BattleStatus = status;
        }

        public TurnOwnerKind TurnOwnerKind => TurnOwnerKind.PLAYER;

        public BattleStatus BattleStatus { get; }

        public string Name => "Player";

        public bool Match(object o)
        {
            return this.Equals(o);
        }
    }


}
