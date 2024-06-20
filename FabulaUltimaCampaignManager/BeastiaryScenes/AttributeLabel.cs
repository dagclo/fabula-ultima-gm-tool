using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class AttributeLabel : Label, IBeastAttribute
{

    [Export]
    public string AttributeName { get; set; }

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
		if (beastTemplate == null) return;
		this.Text = beastTemplate.GetAttributeValue(AttributeName);
    }
}
