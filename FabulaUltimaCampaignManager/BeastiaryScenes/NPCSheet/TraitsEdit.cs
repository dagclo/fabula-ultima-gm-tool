using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class TraitsEdit : LineEdit, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }

    public void OnTextSubmitted(string newText)
    {
        _beastTemplate.Traits = newText;
    }
}
