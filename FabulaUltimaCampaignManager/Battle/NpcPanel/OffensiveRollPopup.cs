using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;
using System.Linq;

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
    }	

	public void UpdateNpcTargets(IEnumerable<NpcInstance> npcs)
	{
        EmitSignal(SignalName.OnNpcTargetListUpdate, new Godot.Collections.Array<NpcInstance>(npcs));
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
