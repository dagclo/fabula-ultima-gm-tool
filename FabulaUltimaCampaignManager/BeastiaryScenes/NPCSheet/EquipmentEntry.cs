using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class EquipmentEntry : PanelContainer
{
    [Signal]
    public delegate void EquipmentSetEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    public void SetEquipment(EquipmentTemplate equipmentEntry)
    {
        var signalObject = new SignalWrapper<EquipmentTemplate>(equipmentEntry);
        EmitSignal(SignalName.EquipmentSet, signalObject);
    }

    public void SetBeastTemplate(IBeastTemplate beastTemplate)
    {
        var signalObject = new SignalWrapper<IBeastTemplate>(beastTemplate);
        EmitSignal(SignalName.BeastSet, signalObject);
    }

    public Action<EquipmentEntry> OnRemoveEquipment { get; set; }

    public void HandleEquipmentRemoved()
    {
        OnRemoveEquipment?.Invoke(this);
    }
}
