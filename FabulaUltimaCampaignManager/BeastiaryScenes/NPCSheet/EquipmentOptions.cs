using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class EquipmentOptions : OptionButton
{   
    private IDictionary<int, EquipmentTemplate> _equipmentMap;

    [Signal]
    public delegate void EquipmentSelectedEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0); // remove unused item
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        
        var index = 0;
        _equipmentMap = new Dictionary<int, EquipmentTemplate>();
        foreach (var equipmentGroupedByCategory in beastRepository.Database.GetEquipmentTemplates().GroupBy(e => e.Category.Id))
        {
            var category = equipmentGroupedByCategory.First().Category;
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
        EmitSignal(SignalName.EquipmentSelected, new SignalWrapper<EquipmentTemplate>(_equipmentMap[index].Clone()));
    }
}
