using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class NpcTabs : TabContainer
{
    private Action RoundChanged { get; set; }
    private IDictionary<string, int> _instanceIdTabIndexMap = new Dictionary<string, int>();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveRoundStateMessage);

        foreach ((int index, var child) in 
            this.GetChildren()
           .Select((p, i) => (i, p)))
        {
            var npcPanel = child as NpcPanel;
            npcPanel.NpcChanged += (NpcInstance npc) => EnableTab(index, npc);
            npcPanel.SetTabTitle += (string s) => SetTabTitle(index, s);
            npcPanel.SetTabIcon += (Texture2D t) => SetTabIcon(index, t);
            this.RoundChanged += npcPanel.OnRoundChanged;
            SetTabHidden(index, true);            
        }
        this.CurrentTab = 0; // fix bizarre issue where this is being set to one for some reason
    }

    private Task ReceiveRoundStateMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> roundStateMessage)) return Task.CompletedTask;      
        RoundChanged?.Invoke();
        return Task.CompletedTask;
    }

    private void EnableTab(int index, NpcInstance npc)
    {
        _instanceIdTabIndexMap[npc.Id] = index;
        SetTabTitle(index, npc.InstanceName);
        SetTabHidden(index, false);
    }
}
