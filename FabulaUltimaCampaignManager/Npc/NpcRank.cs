using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NpcRank : Label, INpcReader
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
        this.Text = npc.Model.Rank.ToString().Replace("_", " ");
    }
}
