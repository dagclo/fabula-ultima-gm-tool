using FirstProject.Npc;
using Godot;
using System;

public partial class WeaponAttributes : Container, INpcEquipmentReader
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
            _basicAttack = equipment.BasicAttack ?? new NpcBasicAttack()
            {
                Id = Guid.NewGuid().ToString()
            };
            _basicAttack.IsRanged = equipment.Category.IsRanged;
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
