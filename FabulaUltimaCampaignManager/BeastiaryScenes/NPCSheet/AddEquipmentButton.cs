using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddEquipmentButton : Button, IBeastAttribute
{   
    private IBeast _beastModel;
    private EquipmentTemplate _equipmentTemplate;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    [Signal]
    public delegate void AddEquipmentEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastModel = beastTemplate.Model;
    }

    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        _equipmentTemplate = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        if (_equipmentTemplate == null) return;
        var clone = _equipmentTemplate.Clone();
        _beastModel.AddEquipment(clone);
        EmitSignal(SignalName.AddEquipment, new SignalWrapper<EquipmentTemplate>(clone));
    }
}
