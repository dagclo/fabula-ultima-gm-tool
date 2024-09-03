using FirstProject.Npc;
using Godot;
using System;

public partial class EquipmentSaveButton : Button, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Disabled = true;
	}

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        this.Disabled = true;
        if (string.IsNullOrWhiteSpace(equipment.Name)) return;
        if (equipment.Cost == default) return;
        if (equipment.Category.Id == default) return;
        this.Disabled = false;
    }

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        // do nothing
    }
}
