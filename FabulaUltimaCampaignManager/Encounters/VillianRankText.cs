using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class VillianRankText : Label, INpcReader
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
		if (!npc.IsVillian) return;
		this.Text = $"{npc.VillainStats.Level}";
    }
}
