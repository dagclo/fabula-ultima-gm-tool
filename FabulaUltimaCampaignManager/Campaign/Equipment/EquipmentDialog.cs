using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class EquipmentDialog : Window
{
	public NpcEquipment Equipment { get; set; }

    [Signal]
    public delegate void EquipmentUpdatedEventHandler(NpcEquipment equipment);

    public Action OnSave { get; set; }
    public Action<NpcEquipment> EquipmentChanged { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if(Equipment == null)
		{
			Equipment = new NpcEquipment();
		}
        foreach (var child in this.FindChildren("*")
            .Where(c => c is INpcEquipmentReader))
        {
            var equipmentAttribute = child as INpcEquipmentReader;
            this.EquipmentChanged += equipmentAttribute.HandleEquipmentSet;
            equipmentAttribute.OnEquipmentUpdated += HandleEquipmentUpdated;

        }
        this.EquipmentChanged?.Invoke(Equipment);
    }

    private void HandleEquipmentUpdated()
    {
        EmitSignal(SignalName.EquipmentUpdated, Equipment);
    }
}

public interface INpcEquipmentReader
{
    void HandleEquipmentSet(NpcEquipment equipment);
    Action OnEquipmentUpdated { get; set; }
}
