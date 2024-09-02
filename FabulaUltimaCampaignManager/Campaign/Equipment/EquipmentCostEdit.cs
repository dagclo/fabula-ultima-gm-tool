using FirstProject.Npc;
using Godot;
using System;

public partial class EquipmentCostEdit : LineEdit, INpcEquipmentReader
{
    private NpcEquipment _equipment;
    private string _currentText;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _currentText = this.Text;
	}

    public void HandleEquipmentSet(NpcEquipment equipment)
    {
        _equipment = equipment;
        if(equipment.Cost != default) _currentText = equipment.Cost.ToString();
        this.Text = _currentText;
    }

    public void HandleTextChanged(string newText)
    {        
        if(int.TryParse(newText, out var cost))
        {
            _equipment.Cost = cost;
            _currentText = this.Text;
            this.OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _currentText;
        }        
    }
}
