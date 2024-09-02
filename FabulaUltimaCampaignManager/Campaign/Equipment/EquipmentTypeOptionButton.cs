using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Npc;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;

public partial class EquipmentTypeOptionButton : OptionButton, INpcEquipmentReader
{
    private NpcEquipment _equipment;
    private System.Collections.Generic.Dictionary<int, NpcEquipmentCategory> _categoriesIndexMap;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        _categoriesIndexMap = beastRepository.Database.GetEquipmentCategories()
            .Select(c => new NpcEquipmentCategory(c))
            .Select((c, i) => (c, i)).ToDictionary(p => p.i, p => p.c);
        foreach(var category in _categoriesIndexMap)
        {
            AddItem(category.Value.Name, category.Key);
        }
        Selected = -1;
    }

    public void HandleEquipmentSet(NpcEquipment equipment)
    {
        _equipment = equipment;
        if(_equipment.Category.Id != null)
        {
            var index = _categoriesIndexMap.Single(p => p.Value.Id == _equipment.Category.Id).Key;
            Selected = index;
        }
    }

    public void HandleSelected(int index)
    {
        var category = _categoriesIndexMap[index];
        _equipment.Category = category;
        this.OnEquipmentUpdated?.Invoke();
    }
}
