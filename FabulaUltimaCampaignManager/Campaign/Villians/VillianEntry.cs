using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class VillianEntry : VBoxContainer
{
    private NPCShortEntry _npcShortEntry;
    private NpcInstance _instance;

    public Action<NpcInstance> OnRemove { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _npcShortEntry = FindChildren("NPCShortEntry").Single<Node>() as NPCShortEntry;
        _npcShortEntry.OnRemove = OnRemove;
        if (_instance != null) _npcShortEntry.UpdateNpc(_instance);

        foreach (var child in FindChildren("Villian*").Where(c => c is INpcReader))
        {
            var label = child as INpcReader;
            if (_instance != null) label.HandleNpcChanged(_instance);
        }
	}

    internal void UpdateNpc(NpcInstance npc)
    {
        _instance = npc;
        npc.VillainStats = npc.VillainStats ?? new VillainStats();
        _npcShortEntry?.UpdateNpc(npc);
    }
}
