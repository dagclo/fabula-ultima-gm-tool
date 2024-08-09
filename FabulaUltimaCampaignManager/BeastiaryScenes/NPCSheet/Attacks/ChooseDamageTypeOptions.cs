using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class ChooseDamageTypeOptions : OptionButton
{
    [Export]
    public Godot.Collections.Array<string> DamageTypes { get; set; } = new Godot.Collections.Array<string>(DamageConstants.DamageTypes);

    [Signal]
    public delegate void DamageTypeChangedEventHandler(string attribute);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach ((var val, int index) in DamageTypes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(0);
    }

    public void OnSelected(int index)
    {
        var attrName = DamageTypes[GetItemId(index)];

        EmitSignal(SignalName.DamageTypeChanged, attrName);
    }
}
