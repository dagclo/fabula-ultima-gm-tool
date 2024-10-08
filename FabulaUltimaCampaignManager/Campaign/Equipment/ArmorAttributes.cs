using FirstProject.Npc;
using Godot;
using System;

public partial class ArmorAttributes : HBoxContainer, INpcEquipmentReader
{
    private NpcEquipmentModifiers _modifiers;

    [Signal]
    public delegate void ModifiersUpdatedEventHandler(NpcEquipmentModifiers equipment);

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        this.Visible = equipment.Category.IsArmor;

        if (this.Visible)
        {
            _modifiers = equipment.Modifiers ?? new NpcEquipmentModifiers()
            {
                
            };
            equipment.Modifiers = _modifiers;
        }
        else
        {
            if (equipment.Modifiers != null) equipment.Modifiers = null;
        }
        EmitSignal(SignalName.ModifiersUpdated, equipment.Modifiers);
    }

    public void HandleEquipmentChanged(NpcEquipment equipment) => HandleEquipmentInitialized(equipment);
}
