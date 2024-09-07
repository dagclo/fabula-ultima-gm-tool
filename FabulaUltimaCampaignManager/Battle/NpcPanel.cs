using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcPanel : PanelContainer, INpcInstanceReader
{
    [Export]
    public int SlotIndex { get; set; }

    [Export]
    public string TurnCounterCharacter { get; set; } = "*";

    [Export]
    public Color ColorMark { get; set; } = Colors.Aqua;

    [Signal]
    public delegate void RoundChangedEventHandler();

    public Action<NpcInstance> NpcChanged { get; set; }
    public Action<string> SetTabTitle { get; set; }
    public Action<Texture2D> SetTabIcon { get; internal set; }

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
        _status.StatusChanged += OnStatusChanged;
        OnStatusChanged(_status);
        //this.SetTabIcon?.Invoke(CreateTexture(ColorMark));
    }

    private static Texture2D CreateTexture(Color colorMark)
    {
        var result = new Texture2D();
        var image = Image.CreateEmpty(20, 20, false, Image.Format.Dxt5);
        
        return result;
    }

    private void OnStatusChanged(BattleStatus status)
    {
        var newTitle = $"{_instance.InstanceName} {string.Join("", Enumerable.Range(1, _status.NumTurnsLeft).Select(_ => TurnCounterCharacter))}";
        SetTabTitle?.Invoke(newTitle);
    }

    internal void OnRoundChanged()
    {        
        if (_instance == null) return;
        if (_status == null) return;
        CallDeferred(MethodName.EmitSignal, SignalName.RoundChanged);
    }
}
