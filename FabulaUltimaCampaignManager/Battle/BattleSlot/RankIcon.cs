using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class RankIcon : TextureRect, INpcReader
{
	[Export]
	public Texture2D Soldier { get; set; }

    [Export]
    public Texture2D Elite { get; set; }


    [Export]
    public Texture2D Champion { get; set; }


    [Export]
    public Texture2D SuperChampion { get; set; }

    [Export]
    public Texture2D UberChampion { get; set; }
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
        switch (npc.Model.Rank)
        {
            case FabulaUltimaNpc.Rank.Soldier:
                this.Texture = Soldier;
                break;
            case FabulaUltimaNpc.Rank.Elite:
                this.Texture = Elite;
                break;
            case FabulaUltimaNpc.Rank.Champion:
                this.Texture = Champion;
                break;
            case FabulaUltimaNpc.Rank.Super_Champion:
                this.Texture = SuperChampion;
                break;
            case FabulaUltimaNpc.Rank.Uber_Champion:
                this.Texture = UberChampion;
                break;
            default:
                break;
        }
    }
    public void StudyLevelChanged(BattleStatus newStatus)
    {
        this.Visible = newStatus.StudyLevel >= BattleStatus.StudyLevelEnum.SOME_INFO;
    }
}
