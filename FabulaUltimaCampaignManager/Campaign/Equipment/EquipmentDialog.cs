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

    public Action OnClose { get; set; }
    public Action<NpcEquipment> OnSave { get; set; }
    private Action<NpcEquipment> EquipmentChanged { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if(Equipment == null)
		{
			Equipment = new NpcEquipment()
            {
                Id = Guid.NewGuid().ToString()
            };
		}
        foreach (var child in this.FindChildren("*")
            .Where(c => c is INpcEquipmentReader))
        {
            var equipmentAttribute = child as INpcEquipmentReader;
            this.EquipmentChanged += equipmentAttribute.HandleEquipmentSet;
            equipmentAttribute.OnEquipmentUpdated += HandleEquipmentUpdated;

        }
        this.EquipmentChanged?.Invoke(Equipment);
        EmitSignal(SignalName.EquipmentUpdated, Equipment);
    }

    private void HandleEquipmentUpdated()
    {
        EmitSignal(SignalName.EquipmentUpdated, Equipment);
    }

    public void HandleClosedRequested()
    {
        OnClose?.Invoke();
    }

    public void HandleSaveButtonPressed()
    {
        OnSave?.Invoke(Equipment);
    }
}

public interface INpcEquipmentReader
{
    void HandleEquipmentSet(NpcEquipment equipment);
    Action OnEquipmentUpdated { get; set; }
}
