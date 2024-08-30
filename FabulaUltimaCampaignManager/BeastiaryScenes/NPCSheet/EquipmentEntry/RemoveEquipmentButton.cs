using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System.Linq;

public partial class RemoveEquipmentButton : Button
{
    private IBeast _beastModel;
    private EquipmentTemplate _equipment;

    [Signal]
    public delegate void EquipmentRemovedEventHandler();

    public void HandleBeastChanged(SignalWrapper<IBeastTemplate> signal)
    {
        _beastModel = signal.Value.Model;
    }

    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        _equipment = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        if (_equipment == null) return;
        var equipmentTemplate = _beastModel.Equipment.FirstOrDefault(e => _equipment == e);
        if (equipmentTemplate == null) return;
        _beastModel.RemoveEquipment(equipmentTemplate);
        EmitSignal(SignalName.EquipmentRemoved);
    }

    public void HandleEquipmentEnable(bool enabled)
    {
        if(_equipment == null) return;
        if (_beastModel == null) return;
        if(enabled)
        {
            if (_beastModel.HasEquipment(_equipment)) return;
            _beastModel.AddEquipment(_equipment);
        }
        else
        {
            if (!_beastModel.HasEquipment(_equipment)) return;
            _beastModel.RemoveEquipment(_equipment);
        }
    }
}
