using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class RankOption : OptionButton, INpcReader
{
    private NpcInstance _instance;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RemoveItem(0);
        foreach (var rank in (FabulaUltimaNpc.Rank[])Enum.GetValues(typeof(FabulaUltimaNpc.Rank))) //todo: set these as constants somewhere else
        {
            AddItem(rank.ToString(), (int) rank);
        }
    }


    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
        var index = GetItemIndex((int) npc.Model.Rank);
        this.Select(index);
    }

    public void OnItemSelected(int index)
    {
        var id = GetItemId(index);
        _instance.Model.Rank = (FabulaUltimaNpc.Rank) id;
    }
}
