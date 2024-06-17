using FirstProject.Npc;
using Godot;
using System;

public partial class DefenseValue : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = npc.Template.Defense.ToString();
    }
}
