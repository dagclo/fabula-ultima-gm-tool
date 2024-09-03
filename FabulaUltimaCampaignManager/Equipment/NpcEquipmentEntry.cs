using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcEquipmentEntry : HBoxContainer
{
    public NpcEquipment Equipment { get; set; }
    private Action<NpcEquipment> EquipmentChanged { get; set; }
    public Action<NpcEquipment> OnShow { get; internal set; }
    public Action<NpcEquipment> OnRemove { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var child in this.FindChildren("*")
            .Where(c => c is INpcEquipmentReader))
        {
            var equipmentAttribute = child as INpcEquipmentReader;
            this.EquipmentChanged += equipmentAttribute.HandleEquipmentInitialized;
        }
        this.EquipmentChanged?.Invoke(Equipment);
    }

    public void HandleShowButtonPressed()
    {
        OnShow?.Invoke(Equipment);
    }

    public void HandleRemoveButtonPressed()
    {
        OnRemove?.Invoke(Equipment);
    }
}
