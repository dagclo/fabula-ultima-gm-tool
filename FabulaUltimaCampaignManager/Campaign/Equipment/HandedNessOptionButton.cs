using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;

public partial class HandedNessOptionButton : OptionButton, INpcEquipmentReader
{
    private NpcEquipment _equipment;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        _equipment = equipment.Category.IsWeapon ? equipment : null;
        this.Visible = _equipment != null;
        if (_equipment == null) return;
        _equipment.NumHands = GetItemId(Selected);
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        _equipment = equipment.Category.IsWeapon ? equipment : null;
        this.Visible = _equipment != null;
        if (_equipment == null) return;
        _equipment.NumHands = GetItemId(Selected);
    }

    public void HandleSelected(int index)
    {
        var numHands = GetItemId(index);
        if (_equipment == null) return;
        _equipment.NumHands = numHands;
        OnEquipmentUpdated?.Invoke();
    }
}
