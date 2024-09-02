using FirstProject.Npc;
using Godot;
using System;

public partial class EquipmentNameEdit : LineEdit, INpcEquipmentReader
{
    private NpcEquipment _equipment;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentSet(NpcEquipment equipment)
    {
        _equipment = equipment;
        this.Text = _equipment.Name;
    }

    public void HandleTextChanged(string newText)
    {
        _equipment.Name = newText;
        this.OnEquipmentUpdated?.Invoke();
    }
}
