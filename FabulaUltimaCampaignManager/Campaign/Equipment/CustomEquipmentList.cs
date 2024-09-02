using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using Godot.Collections;
using System;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

public partial class CustomEquipmentList : VBoxContainer
{
    private Array<NpcEquipment> _equipmentList;
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
    public PackedScene Entry { get; set; }

    [Export]
    public PackedScene EquipmentDialog { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
    }

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        var campaign = signal.Value;
        if (campaign.Equipment == null) campaign.Equipment = new Godot.Collections.Array<NpcEquipment>();
        _equipmentList = campaign.Equipment;
        UpdateList();
    }

    private void UpdateList()
    {
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (_equipmentList?.Any() != true) return;
        foreach (var equipment in _equipmentList)
        {
            var scene = Entry.Instantiate<NpcEquipmentEntry>();
            scene.Equipment = equipment;
            scene.OnShow += (NpcEquipment e) => HandleShow(e);
            scene.OnRemove += (NpcEquipment e) => HandleRemove(e);
            AddChild(scene);
            scene.Owner = this;
        }
    }

    private void HandleRemove(NpcEquipment equipment)
    {
        _equipmentList.Remove(equipment);
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
        UpdateList();
    }

    public void HandlePressed()
    {
        HandleShow();
    }

    public void HandleShow(NpcEquipment equipment = null)
    {
        var dialog = EquipmentDialog.Instantiate<EquipmentDialog>();
        dialog.Equipment = equipment;
        dialog.OnClose += () => HandleClosing(dialog);
        dialog.OnSave += HandleSave;        
        AddChild(dialog);
        dialog.Owner = this;
    }

    private void HandleSave(NpcEquipment equipment)
    {
        if (!_equipmentList.Contains(equipment))
        {
            _equipmentList.Add(equipment);
        }
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
        UpdateList();
    }

    private void HandleClosing(EquipmentDialog dialog)
    {
        RemoveChild(dialog);
        dialog.QueueFree();
    }
}
