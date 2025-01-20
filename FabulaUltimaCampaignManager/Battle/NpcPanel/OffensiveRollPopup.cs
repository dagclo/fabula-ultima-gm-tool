using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;

public partial class OffensiveRollPopup : PopupPanel
{
    [Signal]
    public delegate void OnPlayerListUpdateEventHandler(Godot.Collections.Array<PlayerData> playerList);

    [Signal]
    public delegate void OnNpcTargetListUpdateEventHandler(Godot.Collections.Array<NpcInstance> npcList);

    [Signal]
    public delegate void OnAttackUpdateEventHandler(SignalWrapper<BasicAttackTemplate> signalWrapper);

    [Signal]
    public delegate void OnBattleStatusUpdateEventHandler(BattleStatus status);

    [Signal]
    public delegate void OnNpcUpdateEventHandler(NpcInstance npc);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}
	
	public void UpdatePlayerList(IEnumerable<PlayerData> players)
	{
		EmitSignal(SignalName.OnPlayerListUpdate, new Godot.Collections.Array<PlayerData>(players));
	}

	public void UpdateNpcTargets(IEnumerable<NpcInstance> npcs)
	{
        EmitSignal(SignalName.OnNpcTargetListUpdate, new Godot.Collections.Array<NpcInstance>(npcs));
    }

	public void UpdateNpcData(NpcInstance npc, BattleStatus status, BasicAttackTemplate attack)
	{
        EmitSignal(SignalName.OnAttackUpdate, new SignalWrapper<BasicAttackTemplate>(attack));
        EmitSignal(SignalName.OnBattleStatusUpdate, status);
        EmitSignal(SignalName.OnNpcUpdate, npc);
    }
}
