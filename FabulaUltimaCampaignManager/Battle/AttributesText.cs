using FirstProject.Npc;
using Godot;
using System;

public partial class AttributesText : Label
{
    public void HandleNpcChanged(NpcInstance npc)
    {
        this.Text = $"DEX={npc.Template.Dexterity} INS={npc.Template.Insight} MIG={npc.Template.Might} WLP={npc.Template.WillPower}";
    }
}
