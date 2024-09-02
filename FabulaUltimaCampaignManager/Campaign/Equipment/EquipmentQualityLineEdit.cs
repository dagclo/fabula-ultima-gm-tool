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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleEquipmentSet(NpcEquipment equipment)
    {
        _equipment = equipment;
        this.Text = equipment.Quality;
    }

    public void HandleTextChanged(string newText)
    {
        _equipment.Quality = newText;
        this.OnEquipmentUpdated?.Invoke();
    }
}
