using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class BasicAttackList : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = string.Join(", ", npc.Template.AllAttacks.Select(a => a.Name));
    }
}
