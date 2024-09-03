using FirstProject.Npc;
using Godot;
using System;

public partial class WeaponAttributes : HBoxContainer, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        this.Visible = equipment.Category.IsWeapon;
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        this.Visible = equipment.Category.IsWeapon;
    }
}
