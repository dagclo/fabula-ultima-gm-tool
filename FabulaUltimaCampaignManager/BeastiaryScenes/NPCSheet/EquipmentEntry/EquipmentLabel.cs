using FirstProject.Beastiary;
using Godot;

public partial class EquipmentLabel : Label
{
    public void HandleEquipmentSelected(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> signalWrapper)
    {
        this.Text = signalWrapper.Value.Name;
    }
}
