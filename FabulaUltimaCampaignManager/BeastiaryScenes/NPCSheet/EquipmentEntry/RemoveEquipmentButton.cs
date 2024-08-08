using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class RemoveEquipmentButton : Button
{
    private ICollection<EquipmentTemplate> _equipmentList;
    private EquipmentTemplate _equipment;

    [Signal]
    public delegate void EquipmentRemovedEventHandler();

    public void HandleBeastChanged(SignalWrapper<IBeastTemplate> signal)
    {        
        _equipmentList = signal.Value.Model.Equipment;
    }

    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        _equipment = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        if (_equipment == null) return;
        var equipmentTemplate = _equipmentList.FirstOrDefault(e => _equipment == e);
        if (equipmentTemplate == null) return;
        _equipmentList.Remove(equipmentTemplate);
        EmitSignal(SignalName.EquipmentRemoved);
    }

    public void HandleEquipmentEnable(bool enabled)
    {
        if(_equipment == null) return;
        if (_equipmentList == null) return;
        if(enabled)
        {
            if (_equipmentList.Contains(_equipment)) return;
            _equipmentList.Add(_equipment);
        }
        else
        {
            if (!_equipmentList.Contains(_equipment)) return;
            _equipmentList.Remove(_equipment);
        }
    }
}
