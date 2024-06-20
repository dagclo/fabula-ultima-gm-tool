using FirstProject.Npc;
using Godot;

public partial class TraitsValue : Label
{
	public void HandleNpcChanged(NpcInstance npc)
	{
		this.Text = npc.Template.Traits;
	}
}
