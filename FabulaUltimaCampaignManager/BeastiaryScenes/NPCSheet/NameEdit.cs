using FabulaUltimaNpc;
using Godot;
using System;

public partial class NameEdit : LineEdit, IBeastAttribute
{
    [Export]
    public string AttributeName { get; set; }

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        this.Text = beastTemplate.GetAttributeValue(AttributeName);
    }
}
