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
    [Signal]
    public delegate void OnPlayerListUpdateEventHandler(Godot.Collections.Array<PlayerData> playerList);

    [Signal]
    public delegate void OnNpcTargetListUpdateEventHandler(Godot.Collections.Array<NpcInstance> npcList);

    [Signal]
    public delegate void OnActionUpdateEventHandler(string name);

    [Signal]
    public delegate void OnBattleStatusUpdateEventHandler(BattleStatus status);

    [Signal]
    public delegate void OnNpcUpdateEventHandler(NpcInstance npc);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var players = GetNode<RunState>("/root/RunState").Campaign.Players.Where(p => p.IsValid);
        EmitSignal(SignalName.OnPlayerListUpdate, new Godot.Collections.Array<PlayerData>(players));
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<EncounterInitialize>(UpdateNpcTargets);
    }	

	public Task UpdateNpcTargets(IMessage message)
	{
        if (!(message is IMessage<EncounterInitialize> initialize)) return Task.CompletedTask;
        var npcs = initialize.Npcs.Select(p => p.npc);
        CallDeferred(MethodName.UpdateNpcTargetsInternal, new Godot.Collections.Array<NpcInstance>(npcs));
        return Task.CompletedTask;
    }

    private void UpdateNpcTargetsInternal(Godot.Collections.Array<NpcInstance> npcs)
    {        
        EmitSignal(SignalName.OnNpcTargetListUpdate, npcs);
    }

	public void UpdateNpcStatus(BattleStatus status)
	{        
        EmitSignal(SignalName.OnBattleStatusUpdate, status);        
    }

    public void ReadAttack(BasicAttackTemplate attack)
    {
        EmitSignal(SignalName.OnActionUpdate, attack.Name);
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        EmitSignal(SignalName.OnNpcUpdate, npc);
    }

    public void Read(SpellTemplate spellTemplate)
    {
        EmitSignal(SignalName.OnActionUpdate, spellTemplate.Name);
    }

    public void Read(IBeastTemplate beast)
    {
        throw new System.NotImplementedException();
    }
}
