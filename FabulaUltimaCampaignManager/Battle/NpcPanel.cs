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
    public delegate void TurnChangedEventHandler(SignalWrapper<ITurnOwner> signalWrapper);

    public Action<NpcInstance> NpcChanged { get; set; }
    public Action<ITurnOwner> TurnDone { get; set; }

    private Action<BattleStatus> StatusSet;
    private NpcInstance _instance;
    private BattleStatus _status;
    private ITurnOwner _turnOwner;
  

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
    }

    public void ReadNpc(NpcInstance instance, BattleStatus battleStatus)
    {
        _instance = instance;
        _status = battleStatus;
        NpcChanged?.Invoke(instance);
        StatusSet?.Invoke(battleStatus);
    }

    internal void OnTurnChanged(ITurnOwner owner)
    {        
        if (_instance == null) return;
        if (_status == null) return;
        _turnOwner = owner;
                
        CallDeferred(MethodName.EmitSignal, SignalName.TurnChanged, new SignalWrapper<ITurnOwner>(owner));
        if (_status.IsDead)
        {
            CallDeferred(MethodName.OnTurnEnd);
            //OnTurnEnd(); // immediately end turn            
        }
    }

    public void OnTurnEnd()
    {
        TurnDone?.Invoke(_turnOwner);
    }
}
