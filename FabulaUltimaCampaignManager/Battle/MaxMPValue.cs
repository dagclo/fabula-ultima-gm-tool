using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class MaxMPValue : Label, INpcReader
{
	
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = npc.Template.MagicPoints.ToString();
    }
}
