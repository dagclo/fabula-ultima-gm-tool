using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class DetailsEdit : LineEdit, IBeastAttribute, IValidatable
{
    private IBeastTemplate _beastTemplate;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;
        this.Text = _beastTemplate.Description;
    }

    public void OnTextSubmitted(string newText)
    {
        _beastTemplate.Description = newText;
    }

    string IValidatable.Name => "Description";
    public IEnumerable<TemplateValidation> Validate()
    {
        if (string.IsNullOrWhiteSpace(_beastTemplate.Description)) yield return new TemplateValidation { Level = ValidationLevel.WARNING, Message = "Not Set" };
    }
}
