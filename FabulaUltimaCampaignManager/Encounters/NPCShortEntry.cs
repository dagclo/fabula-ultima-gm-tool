using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NPCShortEntry : VBoxContainer
{
    private NpcInstance _instance;

    public Action<NpcInstance> OnRemove { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var child in this.FindChildren("*")
           .Where(l => l is INpcReader))
        {
            var label = child as INpcReader;
            if(_instance != null) label.HandleNpcChanged(_instance);
        }
    }

    public void UpdateNpc(NpcInstance npc)
    {
        _instance = npc;
    }

    public void HandleRemoveButtonPressed()
    {
        OnRemove?.Invoke(_instance);
    }
}
