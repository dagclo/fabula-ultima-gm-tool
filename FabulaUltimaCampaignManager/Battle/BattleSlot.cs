using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class BattleSlot : Node2D, INpcInstanceReader
{
	[Export]
    public int SlotIndex { get; set; }

    [Signal]
    public delegate void BattleStatusChangedEventHandler(BattleStatus status);

    [Signal]
    public delegate void NpcStudyLevelChangedEventHandler(BattleStatus status);

    private Action<NpcInstance> NpcChanged { get; set; }

    private Sprite2D _sprite;
    private Sprite2D _shadow;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Visible = false;
        foreach (var child in this.FindChildren("*")
           .Where(c => c is INpcReader))
        {   
            var npcReader = child as INpcReader;
            this.NpcChanged += npcReader.HandleNpcChanged;
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ReadNpc(NpcInstance instance, BattleStatus battleStatus)
    {   
        this.Visible = true;
        NpcChanged?.Invoke(instance);
        battleStatus.StatusChanged += this.StatusChanged;
        battleStatus.StudyLevelChanged += this.HandleStudyLevelChanged;
        EmitSignal(SignalName.NpcStudyLevelChanged, battleStatus);
    }

    private void HandleStudyLevelChanged(BattleStatus newStatus)
    {
        EmitSignal(SignalName.NpcStudyLevelChanged, newStatus);
    }

    private void StatusChanged(BattleStatus newStatus)
    {
        EmitSignal(SignalName.BattleStatusChanged, newStatus);
    }
}
