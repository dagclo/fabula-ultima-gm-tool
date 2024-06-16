using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class SpeciesLabel : Label, INpcReader
{
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
		this.Text = npc.Template.Species.Name.ToUpperInvariant();
    }

    public void StudyLevelChanged(BattleStatus newStatus)
    {
        this.Visible = newStatus.StudyLevel >= BattleStatus.StudyLevelEnum.SOME_INFO;
    }
}
