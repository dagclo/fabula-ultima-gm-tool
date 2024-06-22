using FabulaUltimaNpc;
using Godot;
using System;

public partial class DetailsEdit : LineEdit, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;        
    }

    public void OnTextSubmitted(string newText)
    {
        _beastTemplate.Description = newText;
    }
}
