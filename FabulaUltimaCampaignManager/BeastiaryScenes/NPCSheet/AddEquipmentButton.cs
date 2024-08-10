using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddEquipmentButton : Button, IBeastAttribute
{   
    private ICollection<EquipmentTemplate> _equipmentList;
    private EquipmentTemplate _equipmentTemplate;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    [Signal]
    public delegate void AddEquipmentEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {   
        _equipmentList = beastTemplate.Model.Equipment;
    }

    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        _equipmentTemplate = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        if (_equipmentTemplate == null) return;
        var clone = _equipmentTemplate.Clone();
        _equipmentList.Add(clone);
        EmitSignal(SignalName.AddEquipment, new SignalWrapper<EquipmentTemplate>(clone));
    }
}
