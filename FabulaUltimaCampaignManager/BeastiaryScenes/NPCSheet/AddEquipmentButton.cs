using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AddEquipmentButton : Button, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;
    private FabulaUltimaDatabase.Models.EquipmentEntry _equipmentEntry;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    [Signal]
    public delegate void AddEquipmentEventHandler(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> equipment);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }

    public void HandleEquipmentSelected(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> signalWrapper)
    {
        _equipmentEntry = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        if (_equipmentEntry == null) return;
        EmitSignal(SignalName.AddEquipment, new SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry>(_equipmentEntry));
    }
}
