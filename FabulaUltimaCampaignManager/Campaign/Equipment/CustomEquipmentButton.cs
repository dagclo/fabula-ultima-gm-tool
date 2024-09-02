using FirstProject.Beastiary;
using FirstProject.Campaign;
using Godot;
using System;

public partial class CustomEquipmentButton : Button
{
    private CampaignData _campaign;

    [Export]
	public PackedScene EquipmentDialog { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        _campaign = signal.Value;
    }

    public void HandlePressed()
    {
        var dialog = EquipmentDialog.Instantiate<EquipmentDialog>();
        dialog.OnClose += () => HandleClosing(dialog);
        AddChild(dialog);
    }

    private void HandleClosing(EquipmentDialog dialog)
    {
        RemoveChild(dialog);
        dialog.QueueFree();
    }
}
