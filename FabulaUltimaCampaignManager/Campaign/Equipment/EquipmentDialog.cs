using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class EquipmentDialog : Window
{
	public NpcEquipment Equipment { get; set; }

    public Action OnClose { get; set; }
    public Action<NpcEquipment> OnSave { get; set; }
    private Action<NpcEquipment> EquipmentInitialized { get; set; }
    public Action<NpcEquipment> EquipmentChanged { get; private set; }

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
            this.EquipmentInitialized += equipmentAttribute.HandleEquipmentInitialized;
            equipmentAttribute.OnEquipmentUpdated += HandleEquipmentUpdated;
            this.EquipmentChanged += equipmentAttribute.HandleEquipmentChanged;

        }
        this.EquipmentInitialized?.Invoke(Equipment);
        this.EquipmentChanged?.Invoke(Equipment);
        this.ResizeForResolution();
    }

    private void HandleEquipmentUpdated()
    {
        this.EquipmentChanged?.Invoke(Equipment);
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
    void HandleEquipmentInitialized(NpcEquipment equipment);
    void HandleEquipmentChanged(NpcEquipment equipment);
    Action OnEquipmentUpdated { get; set; }
}
