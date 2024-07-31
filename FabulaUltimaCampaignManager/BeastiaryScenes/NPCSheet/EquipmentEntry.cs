using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class EquipmentEntry : PanelContainer
{
    [Signal]
    public delegate void EquipmentSetEventHandler(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> equipment);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> equipment);

    public void SetEquipment(FabulaUltimaDatabase.Models.EquipmentEntry equipmentEntry)
    {
        var signalObject = new SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry>(equipmentEntry);
        EmitSignal(SignalName.EquipmentSet, signalObject);
    }

    public void SetBeastTemplate(IBeastTemplate beastTemplate)
    {
        var signalObject = new SignalWrapper<IBeastTemplate>(beastTemplate);
        EmitSignal(SignalName.BeastSet, signalObject);
    }
}
