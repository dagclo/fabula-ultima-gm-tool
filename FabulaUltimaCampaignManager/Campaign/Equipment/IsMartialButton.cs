using FirstProject.Npc;
using Godot;
using System;

public partial class IsMartialButton : CheckButton, INpcEquipmentReader
{
    private NpcEquipment _equipment;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        _equipment = equipment;
        SetPressedNoSignal(equipment.IsMartial);        
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        // do nothing
    }

    public void HandleToggle(bool toggle)
    {
        _equipment.IsMartial = toggle;
        this.OnEquipmentUpdated?.Invoke();
    }
}
