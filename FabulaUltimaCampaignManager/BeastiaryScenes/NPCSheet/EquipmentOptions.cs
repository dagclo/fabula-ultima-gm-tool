using FabulaUltimaDatabase.Models;
using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EquipmentOptions : OptionButton
{   
    private IDictionary<int, FabulaUltimaDatabase.Models.EquipmentEntry> _equipmentMap;

    [Signal]
    public delegate void EquipmentSelectedEventHandler(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> equipment);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0); // remove unused item
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        var equipmentCategory = beastRepository.Database.GetEquipmentCategories().ToDictionary(c => c.Id, c => c);
        var index = 0;
        _equipmentMap = new Dictionary<int, FabulaUltimaDatabase.Models.EquipmentEntry>();
        foreach (var equipmentGroupedByCategory in beastRepository.Database.GetEquipment().GroupBy(e => e.CategoryId))
        {
            var category = equipmentCategory[equipmentGroupedByCategory.Key.Value];
            AddItem($"===={category.Name}====", index);
            SetItemDisabled(index, true);
            index++;
            foreach(var equipment in equipmentGroupedByCategory.OrderBy(e => e.Name))
            {
                AddItem(equipment.Name, index);
                _equipmentMap[index] = equipment;
                index++;
            }            
        }
        this.Selected = -1;
    }

    public void HandleEquipmentSelected(int index)
    {
        EmitSignal(SignalName.EquipmentSelected, new SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry>(_equipmentMap[index]));
    }
}
