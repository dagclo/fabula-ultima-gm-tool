using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class SpellAttributeRollOptions : OptionButton
{
    private SpellTemplate _spell;

    [Export]
    public int Index { get; set; }

    [Export]
    public Godot.Collections.Array<string> Attributes { get; set; } = new Godot.Collections.Array<string>(AttributeExtensions.ShortAttributeNames);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddItem("Unset", -1);
        foreach ((var val, int index) in Attributes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(-1);
    }

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        _spell = signal.Value;
        if (_spell == null) return;
        if (!editable)
        {
            var spellAttribute = Index == 0 ? _spell.Attribute1 : _spell.Attribute2;
            if (spellAttribute == null)
            {
                this.Visible = false;
                return;
            }
            var shortenedSpellAttribute = spellAttribute.ShortenAttribute();
            this.Visible = true;
            int selectedIndex = -1;
            foreach ((var attr, var index) in Attributes.Select((a, i) => (a, i)))
            {
                if (string.Equals(attr, shortenedSpellAttribute, StringComparison.InvariantCultureIgnoreCase))
                {
                    selectedIndex = index;
                    break;
                }
            }
            Select(selectedIndex);
        }
        else
        {
            this.Visible = true;
            this.Disabled = false;
        }       
    }

    public void HandleSelected(int index)
    {
        var shortAttributeName = GetItemText(index);
        if(Index == 0)
        {
            _spell.Attribute1 = shortAttributeName.LengthenAttributeName();
        }
        else
        {
            _spell.Attribute2 = shortAttributeName.LengthenAttributeName();
        }
    }
}
