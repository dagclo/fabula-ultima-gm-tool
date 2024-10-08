using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NPCShortEntry : VBoxContainer
{
    private NpcInstance _instance;

    public Action<NpcInstance> OnRemove { get; internal set; }

    [Export]
    public bool ShowVillianRank { get; set; } = true;

    [Signal]
    public delegate void VillianRankStatusEventHandler(bool show);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        foreach (var child in this.FindChildren("*")
           .Where(l => l is INpcReader))
        {
            var label = child as INpcReader;
            if (_instance != null) label.HandleNpcChanged(_instance);
        }
        EmitSignal(SignalName.VillianRankStatus, ShowVillianRank);
    }

    public void UpdateNpc(NpcInstance npc)
    {
        _instance = npc;
        UpdateChildren();
    }

    public void HandleRemoveButtonPressed()
    {
        OnRemove?.Invoke(_instance);
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
                
        var dragPreview = new Label();
        dragPreview.Text = _instance.Model.Name;
        SetDragPreview(dragPreview);
        return _instance;
    }
}
