using FabulaUltimaDatabase;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcPanel : PanelContainer, INpcInstanceReader
{
    [Export]
    public int SlotIndex { get; set; }

    [Signal]
    public delegate void RoundChangedEventHandler();

    public Action<NpcInstance> NpcChanged { get; set; }
    
    private Action<BattleStatus> StatusSet;
    private NpcInstance _instance;
    private BattleStatus _status;    
  

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{        
        foreach (var child in this.FindChildren("*")
         .Where(l => l is INpcReader))
        {
            var attributePanel = child as INpcReader;
            this.NpcChanged += attributePanel.HandleNpcChanged;
        }

        foreach (var child in this.FindChildren("*")
         .Where(l => l is INpcStatusReader))
        {
            var statusReader = child as INpcStatusReader;
            this.StatusSet += statusReader.HandleStatusSet;
        }
    }

    public void ReadNpc(NpcInstance instance, BattleStatus battleStatus)
    {
        _instance = instance;
        _status = battleStatus;
        NpcChanged?.Invoke(instance);
        StatusSet?.Invoke(battleStatus);
    }

    internal void OnRoundChanged()
    {        
        if (_instance == null) return;
        if (_status == null) return;
        CallDeferred(MethodName.EmitSignal, SignalName.RoundChanged);
    }
}
