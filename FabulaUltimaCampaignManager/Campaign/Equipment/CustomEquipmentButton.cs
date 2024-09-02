using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class CustomEquipmentButton : Button
{
    private Godot.Collections.Array<NpcEquipment> _equipmentList;
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
	public PackedScene EquipmentDialog { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");        
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        var campaign = signal.Value;
        if (campaign.Equipment == null) campaign.Equipment = new Godot.Collections.Array<NpcEquipment>();
        _equipmentList = signal.Value.Equipment;
    }

    public void HandlePressed()
    {
        var dialog = EquipmentDialog.Instantiate<EquipmentDialog>();
        dialog.OnClose += () => HandleClosing(dialog);
        dialog.OnSave += HandleSave;
        AddChild(dialog);
    }

    private void HandleSave(NpcEquipment equipment)
    {
        if (!_equipmentList.Contains(equipment))
        {
            _equipmentList.Add(equipment);
        }
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

    private void HandleClosing(EquipmentDialog dialog)
    {
        RemoveChild(dialog);
        dialog.QueueFree();
    }
}
