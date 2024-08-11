using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class NameEdit : LineEdit, IBeastAttribute, IValidatable
{
    private IBeastTemplate _beastTemplate;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    string IValidatable.Name => "NPC Name";

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;        
    }

    public void OnTextSubmitted(string newText)
    {
        _beastTemplate.Name = newText;
    }

    public IEnumerable<TemplateValidation> Validate()
    {
        if(string.IsNullOrWhiteSpace(_beastTemplate.Name)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Name Not Set" };
    }
}
