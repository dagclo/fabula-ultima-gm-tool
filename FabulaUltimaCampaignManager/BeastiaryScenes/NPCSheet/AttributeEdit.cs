using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AttributeEdit : Control, IBeastAttribute, IValidatable
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public string AttributeName { get; set; }

    [Signal]
    public delegate void AttributeNameSetEventHandler(string attributeName);

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;
        EmitSignal(SignalName.AttributeNameSet, AttributeName.ShortenAttribute());
    }

    public void HandleDiceSizeChanged(int newSize)
    {
        if(_beastTemplate == null) return;
        switch(AttributeName.ToLowerInvariant())
        {
            case "might":
                _beastTemplate.Might = new Die(newSize); 
                break;
            case "insight":
                _beastTemplate.Insight = new Die(newSize);
                break;
            case "willpower":
                _beastTemplate.WillPower = new Die(newSize);
                break;
            case "dexterity":
                _beastTemplate.Dexterity = new Die(newSize);
                break;
            default:
                return;
        }
        BeastTemplateAction.Invoke(new HashSet<BeastEntryNode.Action>() { BeastEntryNode.Action.CHANGED });
    }

    string IValidatable.Name => AttributeName;
    public IEnumerable<TemplateValidation> Validate()
    {
        if (_beastTemplate == null) yield break;
        Die attributeDieValue;
        switch (AttributeName.ToLowerInvariant())
        {
            case "might":
                attributeDieValue = _beastTemplate.Might;
                break;
            case "insight":
                attributeDieValue = _beastTemplate.Insight;
                break;
            case "willpower":
                attributeDieValue = _beastTemplate.WillPower;
                break;
            case "dexterity":
                attributeDieValue = _beastTemplate.Dexterity;
                break;
            default:
                attributeDieValue = default;
                break;
        }
        if (attributeDieValue.Sides.Equals(default(Die).Sides)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Not Set" };
    }
}
