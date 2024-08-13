using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class EquipmentEntry : PanelContainer
{
    private bool? _canUseEquipment = null;

    [Signal]
    public delegate void EquipmentSetEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<EquipmentTemplate> equipment);

    [Signal]
    public delegate void CanBeastUseEquipmentEventHandler(bool useEquipment);

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

    internal void HandleEquipmentSkillChanged(bool enable)
    {
        if (enable == _canUseEquipment) return; // only emit signal if things have changed
        EmitSignal(SignalName.CanBeastUseEquipment, enable);
        _canUseEquipment = enable;
    }
}
