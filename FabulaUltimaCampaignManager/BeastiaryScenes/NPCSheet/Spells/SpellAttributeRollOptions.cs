using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class SpellAttributeRollOptions : OptionButton
{
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

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal)
    {
        var spell = signal.Value;
        var spellAttribute = Index == 0 ? spell.Attribute1 : spell.Attribute2;
        spellAttribute = spellAttribute.ShortenAttribute();
        if (spellAttribute == null) return;
        int selectedIndex = -1;
        foreach((var attr, var index) in Attributes.Select((a, i) => (a, i)))
        {
            if(string.Equals(attr, spellAttribute, StringComparison.InvariantCultureIgnoreCase))
            {
                selectedIndex = index;
                break;
            }
        }
        Select(selectedIndex);
    }
}
