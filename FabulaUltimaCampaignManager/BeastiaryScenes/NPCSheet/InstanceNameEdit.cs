using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;

public partial class InstanceNameEdit : LineEdit, IBeastAttribute, IValidatable
{
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    string IValidatable.Name => "Instance Name";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Visible = false;
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var npcModel = beastTemplate.Model as NpcModel;
        if (npcModel == null) return;      
        this.Visible = true;
    }

    public IEnumerable<TemplateValidation> Validate()
    {
        if (!this.Visible) yield break;
        if (string.IsNullOrWhiteSpace(Text)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Name should be set" };
    }
}
