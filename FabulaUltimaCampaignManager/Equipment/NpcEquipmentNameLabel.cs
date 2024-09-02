using FirstProject.Npc;
using Godot;
using System;

public partial class NpcEquipmentNameLabel : Label, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentSet(NpcEquipment equipment)
    {
        this.Text = equipment.Name;
    }
}
