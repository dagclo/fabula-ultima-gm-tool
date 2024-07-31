using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class EquipmentLabel : Label
{
    private IBeastTemplate _beastTemplate;

    public void HandleEquipmentSelected(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> signalWrapper)
    {
        this.Text = signalWrapper.Value.Name;
    }
}
