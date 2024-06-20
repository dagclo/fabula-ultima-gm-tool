using FirstProject.Npc;
using Godot;

public partial class MDefValue : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = npc.Template.MagicalDefense.ToString();
    }
}
