using FirstProject.Npc;
using Godot;
using System;

public partial class EquipmentQualityLineEdit : TextEdit, INpcEquipmentReader
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
        this.Text = equipment.Quality;
    }

    public void HandleTextChanged()
    {
        _equipment.Quality = this.Text;
        this.OnEquipmentUpdated?.Invoke();
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        // do nothing
    }
}
