using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class SpeciesSelect : OptionButton, IBeastAttribute
{
    [Signal]
    public delegate void SpeciesUpdateEncounterEventHandler(SignalWrapper<IEnumerable<SkillTemplate>> speciesSkills);
    private IBeastTemplate _beastTemplate;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0);
        foreach (var val in new[] { "Beast", "Construct", "Demon", "Elemental", "Humanoid", "Monster", "Plant", "Undead" })
        {
            AddItem(val);
        }
    }

    public void HandleItemSelect(int index)
    {
        if (_beastTemplate == null) return;
        var species = GetItemText(index);
        _beastTemplate.Species = new SpeciesType(SpeciesConstants.FromString(species), species);
        BeastTemplateAction.Invoke(new HashSet<BeastEntryNode.Action>() { BeastEntryNode.Action.TRIGGER});
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }
}
