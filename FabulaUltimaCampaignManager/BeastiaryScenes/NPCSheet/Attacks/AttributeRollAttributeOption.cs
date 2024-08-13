using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class AttributeRollAttributeOption : OptionButton
{
    private BasicAttackTemplate _basicAttack;

    [Export]
    public int Index { get; set; }

    [Export]
    public Godot.Collections.Array<string> Attributes { get; set; } = new Godot.Collections.Array<string>(AttributeExtensions.ShortAttributeNames);
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach ((var val, int index) in Attributes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(-1);
    }

    public void OnSelected(int index)
    {
        var attrName = Attributes[GetItemId(index)];
        var longAttributeName = attrName.LengthenAttributeName();
        switch(Index)
        {
            case 0:
                _basicAttack.Attribute1 = longAttributeName;
                break;
            case 1:
                _basicAttack.Attribute2 = longAttributeName;
                break;
            default:
                throw new ArgumentOutOfRangeException($"{Index} out of range");
        }        
    }

    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        _basicAttack = attack;
    }
}
