using FirstProject.Npc;
using Godot;

public partial class DefenseValue : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = npc.Template.Defense.ToString();
    }
}
