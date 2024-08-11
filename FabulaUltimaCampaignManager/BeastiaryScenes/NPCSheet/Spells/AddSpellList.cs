using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AddSpellList : VBoxContainer, IBeastAttribute
{
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Visible = false;
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = beastTemplate.Skills.Any(s => s.IsSpellcasterSkill());
    }
}
