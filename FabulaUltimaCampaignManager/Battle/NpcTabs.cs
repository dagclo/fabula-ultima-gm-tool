using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class NpcTabs : TabContainer
{
    private MessagePublisher<TurnDoneMessage> _messagePublisher;

    private Action<ITurnOwner> TurnChanged { get; set; }
    private IDictionary<string, int> _instanceIdTabIndexMap = new Dictionary<string, int>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveRoundStateMessage);
        _messagePublisher = messageRouter.GetPublisher<TurnDoneMessage>();

        foreach ((int index, var child) in 
            this.GetChildren()
           .Select((p, i) => (i, p)))
        {
            var npcPanel = child as NpcPanel;
            npcPanel.NpcChanged += (NpcInstance npc) => EnableTab(index, npc);
            this.TurnChanged += npcPanel.OnTurnChanged;
            npcPanel.TurnDone += this.OnTurnDone;
            SetTabHidden(index, true);            
        }
    }

    private void OnTurnDone(ITurnOwner turnOwner)
    {
        var turnDoneMessage = new TurnDoneMessage
        {
            TurnOwner = turnOwner
        };
        _messagePublisher.Publish(turnDoneMessage.AsMessage());
    }

    private Task ReceiveRoundStateMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> roundStateMessage)) return Task.CompletedTask;
        var roundState = roundStateMessage.Value;
        if (roundState.CurrentTurnOwner.TurnOwnerKind != FirstProject.Npc.TurnOwnerKind.NPC) return Task.CompletedTask;
        var npc = (NpcTurnOwner)roundState.CurrentTurnOwner;
        CallDeferred(NpcTabs.MethodName.SwitchTab, _instanceIdTabIndexMap[npc.Instance.Id]);
        TurnChanged?.Invoke(roundState.CurrentTurnOwner);
        return Task.CompletedTask;
    }

    private void SwitchTab(int index)
    {
        this.CurrentTab = index;
    }

    private void EnableTab(int index, NpcInstance npc)
    {
        _instanceIdTabIndexMap[npc.Id] = index;
        SetTabTitle(index, npc.InstanceName);
        SetTabHidden(index, false);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
