using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class EquipmentLabel : Label
{
    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        this.Text = signalWrapper.Value.Name;
    }
}
