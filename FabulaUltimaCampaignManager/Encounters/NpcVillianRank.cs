using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NpcVillianRank : PanelContainer, INpcReader
{
    private NpcInstance _instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Visible = false;
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
    }

	public void HandleVillianRankStatus(bool show)
	{
		this.Visible = show && (_instance?.IsVillian ?? false);
	}
}
