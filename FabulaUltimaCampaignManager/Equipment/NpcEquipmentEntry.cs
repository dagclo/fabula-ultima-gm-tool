using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcEquipmentEntry : HBoxContainer
{
    public NpcEquipment Equipment { get; set; }
    private Action<NpcEquipment> EquipmentChanged { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var child in this.FindChildren("*")
            .Where(c => c is INpcEquipmentReader))
        {
            var equipmentAttribute = child as INpcEquipmentReader;
            this.EquipmentChanged += equipmentAttribute.HandleEquipmentSet;
        }
        this.EquipmentChanged?.Invoke(Equipment);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
