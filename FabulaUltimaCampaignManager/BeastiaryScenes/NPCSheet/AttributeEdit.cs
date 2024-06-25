using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AttributeEdit : Control, IBeastAttribute
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
}
