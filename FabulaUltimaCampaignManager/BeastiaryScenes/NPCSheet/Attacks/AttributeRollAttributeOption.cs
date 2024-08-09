using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class AttributeRollAttributeOption : OptionButton
{
    [Export]
    public int Index { get; set; }

    [Export]
    public Godot.Collections.Array<string> Attributes { get; set; } = new Godot.Collections.Array<string>(AttributeExtensions.ShortAttributeNames);

    [Signal]
    public delegate void AttributeChangedEventHandler(string attribute, int index);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach ((var val, int index) in Attributes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(0);
    }

    public void OnSelected(int index)
    {
        var attrName = Attributes[GetItemId(index)];

        EmitSignal(SignalName.AttributeChanged, attrName, Index);
    }
}
