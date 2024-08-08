using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using Godot;
using System;
using System.Collections.Generic;

public partial class EquipmentContainer : HBoxContainer, IBeastAttribute
{
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

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
        this.Visible = KnownSkills.UseEquipment.SpeciesCanUse(beastTemplate);
    }
}
