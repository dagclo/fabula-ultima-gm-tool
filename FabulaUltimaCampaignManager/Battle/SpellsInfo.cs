using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class SpellsInfo : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = string.Join(", ", npc.Template.Spells.Select(s => s.Name));
    }
}
