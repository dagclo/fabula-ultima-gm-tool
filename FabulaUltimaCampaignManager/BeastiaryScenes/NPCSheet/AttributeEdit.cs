using FabulaUltimaNpc;
using Godot;
using System;

public partial class AttributeEdit : Control, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public string AttributeName { get; set; }

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;
    }
}
