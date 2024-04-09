using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class NPCShortEntry : VBoxContainer
{

    private NpcInstance _instance;

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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void UpdateNpc(NpcInstance npc)
    {
        _instance = npc;
    }
}
