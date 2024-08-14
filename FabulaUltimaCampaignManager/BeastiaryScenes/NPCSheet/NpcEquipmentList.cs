using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
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
    public Action<bool> OnEquipmentSkillChanged { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleEquipmentSelected(SignalWrapper<EquipmentTemplate> signalWrapper)
    {
        var equipmentEntry = signalWrapper.Value;
        var scene = EquipmentEntryScene.Instantiate<EquipmentEntry>();
        scene.SetEquipment( equipmentEntry);
        scene.SetBeastTemplate(_beastTemplate);
        scene.OnRemoveEquipment += HandleRemoveEquipment;
        OnEquipmentSkillChanged += scene.HandleEquipmentSkillChanged;
        AddChild(scene);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action> { BeastEntryNode.Action.TRIGGER });
    }

    private void HandleRemoveEquipment(EquipmentEntry entry)
    {
        this.RemoveChild(entry);
        entry.QueueFree();
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action> { BeastEntryNode.Action.TRIGGER });
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;        
        OnEquipmentSkillChanged?.Invoke(KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate));
    }    
}