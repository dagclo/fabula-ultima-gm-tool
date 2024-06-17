using FirstProject.Npc;
using Godot;
using System;

public partial class MDefValue : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = npc.Template.MagicalDefense.ToString();
    }
}
