using FirstProject.Messaging;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class PlayersList : Container
{   
    private Action RoundStart { get; set; }    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveRoundStateMessage);

        var players = GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid).ToArray();
		var playerNodes = this.FindChildren("*")
			.Where(p => p is IPlayerTurnHandler)
			.Select(p => (IPlayerTurnHandler) p)
			.Take(players.Count())
            .ToArray();

		
        foreach ((var player, var playerPanel) in players.Zip(playerNodes))
		{
			playerPanel.ReadPlayer(player);            
            this.RoundStart += playerPanel.OnRoundStart;
        }
	}

    private int? _lastKnownRoundNumber = null;
    private Task ReceiveRoundStateMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> roundStateMessage)) return Task.CompletedTask;
        var roundState = roundStateMessage.Value;
        RoundStart?.Invoke();
        return Task.CompletedTask;
    }
}
