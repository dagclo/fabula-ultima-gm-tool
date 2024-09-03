using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;

public partial class WeaponAttributes : HBoxContainer, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }
    private NpcBasicAttack _basicAttack;

    [Signal]
    public delegate void BasicAttackUpdatedEventHandler(NpcBasicAttack equipment);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        this.Visible = equipment.Category.IsWeapon;
        if (this.Visible)
        {
            _basicAttack = _basicAttack ?? new NpcBasicAttack()
            {
                Id = Guid.NewGuid().ToString()
            };
            equipment.BasicAttack = _basicAttack;
        }
        else
        {
            if(equipment.BasicAttack != null) equipment.BasicAttack = null;
        }
        EmitSignal(SignalName.BasicAttackUpdated, equipment.BasicAttack);
    }

    public void HandleEquipmentChanged(NpcEquipment equipment) => HandleEquipmentInitialized(equipment);
}
