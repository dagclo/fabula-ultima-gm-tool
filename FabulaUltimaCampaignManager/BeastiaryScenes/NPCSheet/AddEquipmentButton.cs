using FabulaUltimaDatabase.Models;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddEquipmentButton : Button, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;
    private EquipmentEntry _equipmentEntry;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

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

    public void HandleEquipmentSelected(SignalWrapper<EquipmentEntry> signalWrapper)
    {
        _equipmentEntry = signalWrapper.Value;
    }

    public void HandlePressed()
    {
        
    }
}
