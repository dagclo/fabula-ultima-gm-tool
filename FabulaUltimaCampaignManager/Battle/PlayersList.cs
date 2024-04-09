using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class PlayersList : VBoxContainer
{
    private MessagePublisher<TurnDoneMessage> _messagePublisher;
    private Action RoundStart { get; set; }
    private Action TurnStart { get; set; }
    private Action RoundEnd { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveRoundStateMessage);
        _messagePublisher = messageRouter.GetPublisher<TurnDoneMessage>();

        var players = GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid).ToArray();
		var playerNodes = this.FindChildren("*")
			.Where(p => p is IPlayerTurnHandler)
			.Select(p => (IPlayerTurnHandler) p)
			.Take(players.Count())
            .ToArray();

		
        foreach ((var player, var playerPanel) in players.Zip(playerNodes))
		{
			playerPanel.ReadPlayer(player);
            playerPanel.CompletedTurn += this.OnCompletedTurn;
            this.RoundStart += playerPanel.OnRoundStart;
            this.TurnStart += playerPanel.OnTurnStart;
            this.RoundEnd += playerPanel.OnRoundEnd;
        }
	}

    private void OnCompletedTurn()
    {
        RoundEnd?.Invoke();
        var turnDoneMessage = new TurnDoneMessage
        {
            TurnOwner = _lastOwner ?? throw new Exception("turn owner not set")
        };
        _messagePublisher.Publish(turnDoneMessage.AsMessage());
    }

    private int? _lastKnownRoundNumber = null;
    private ITurnOwner _lastOwner;
    private Task ReceiveRoundStateMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> roundStateMessage)) return Task.CompletedTask;
        var roundState = roundStateMessage.Value;
        if(_lastKnownRoundNumber != roundState.RoundNumber)
        {
            _lastKnownRoundNumber = roundState.RoundNumber;
            RoundStart?.Invoke();
        }
        if(roundState.CurrentTurnOwner.TurnOwnerKind == FirstProject.Npc.TurnOwnerKind.PLAYER)
        {
            //todo check if player is dead or incapacitated
            TurnStart?.Invoke();
            _lastOwner = roundState.CurrentTurnOwner;
        }
        else
        {
            _lastOwner = null;
        }
        return Task.CompletedTask;
    }
}
