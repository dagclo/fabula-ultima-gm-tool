using FabulaUltimaGMTool.Battle;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class OffensiveRollPopup : PopupPanel, IAttackReader, INpcReader, ISpellReader
{
    private NpcInstance _npc;

    [Signal]
    public delegate void OnPlayerListUpdateEventHandler(Godot.Collections.Array<PlayerData> playerList);

    [Signal]
    public delegate void OnNpcTargetListUpdateEventHandler(Godot.Collections.Array<NpcInstance> npcList);

    [Signal]
    public delegate void OnActionUpdateEventHandler(string name, string type, string defense);

    [Signal]
    public delegate void OnNpcUpdateEventHandler(NpcInstance npc);

    [Signal]
    public delegate void OnStatusUpdateEventHandler(BattleStatus status);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var players = GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid);
        EmitSignal(SignalName.OnPlayerListUpdate, new Godot.Collections.Array<PlayerData>(players));
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<EncounterInitialize>(ReceiveMessage);
    }	

	public Task ReceiveMessage(IMessage message)
	{
        if (_npc == null || !(message is IMessage<EncounterInitialize> initialize)) return Task.CompletedTask;
        var otherNpcs = initialize.Value.Npcs.Select(p => p.npc).Where(n => n.Id != _npc.Id );
        var statuses = initialize.Value.Npcs.Select(p => p.status);
        var npcStatus = statuses.Single(s => s.ToString() == _npc.Id);
        CallDeferred(MethodName.Update, new Godot.Collections.Array<NpcInstance>(otherNpcs), npcStatus);
        return Task.CompletedTask;
    }

    private void Update(Godot.Collections.Array<NpcInstance> npcs, BattleStatus status)
    {        
        EmitSignal(SignalName.OnNpcTargetListUpdate, npcs);
        EmitSignal(SignalName.OnStatusUpdate, status);
    }

    public void ReadAttack(BasicAttackTemplate attack)
    {
        EmitSignal(SignalName.OnActionUpdate, attack.Name, "Attack", "Def");
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npc = npc;
        EmitSignal(SignalName.OnNpcUpdate, npc);
    }

    public void Read(SpellTemplate spellTemplate)
    {
        EmitSignal(SignalName.OnActionUpdate, spellTemplate.Name, "Spell", "M Def");
    }

    public void Read(IBeastTemplate beast)
    {
        throw new System.NotImplementedException();
    }

    public void HandleOpen()
    {
        this.Visible = true;
    }
}
