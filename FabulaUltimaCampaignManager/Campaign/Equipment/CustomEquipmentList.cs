using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Npc;
using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class CustomEquipmentList : VBoxContainer
{
    private Array<NpcEquipment> _equipmentList;

    [Export]
    public PackedScene Entry { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
    }

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {     
        _equipmentList = signal.Value.Equipment;
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (_equipmentList?.Any() != true) return;
        foreach(var equipment in _equipmentList)
        {
            var scene = Entry.Instantiate<NpcEquipmentEntry>();
            scene.Equipment = equipment;
            AddChild(scene);
        }
    }
}
