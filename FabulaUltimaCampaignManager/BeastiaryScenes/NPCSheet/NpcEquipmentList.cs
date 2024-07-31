using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class NpcEquipmentList : Container, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public PackedScene EquipmentEntryScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleEquipmentSelected(SignalWrapper<FabulaUltimaDatabase.Models.EquipmentEntry> signalWrapper)
    {
        var equipmentEntry = signalWrapper.Value;
        var scene = EquipmentEntryScene.Instantiate<EquipmentEntry>();
        scene.SetEquipment( equipmentEntry);
        scene.SetBeastTemplate(_beastTemplate);
        AddChild(scene);
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }
}
