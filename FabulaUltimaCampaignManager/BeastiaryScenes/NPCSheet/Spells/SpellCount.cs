using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpellCount : Label, IBeastAttribute, IValidatable
{
    private int _spellSlotsAvailable;
    private int _spellSlotsUsed;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    [Signal]
    public delegate void UpdateSpellEnabledEventHandler(bool addSpellEnabled);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Text = "0/0";
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var spellSkills = beastTemplate.Skills.Where(s => s.IsSpellcasterSkill());
        _spellSlotsAvailable = spellSkills.Select(s => int.Parse(s.OtherAttributes[StatsConstants.NUM_SPELLS])).Sum();
        _spellSlotsUsed = beastTemplate.Spells.Count();
        this.Text = $"{_spellSlotsUsed}/{_spellSlotsAvailable}";
        EmitSignal(SignalName.UpdateSpellEnabled, _spellSlotsUsed < _spellSlotsAvailable);
    }

    string IValidatable.Name => "Spell Count";
    public IEnumerable<TemplateValidation> Validate()
    {
        if (_spellSlotsUsed > _spellSlotsAvailable) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Too Many Spells: Remove {_spellSlotsUsed - _spellSlotsAvailable}" };
        if (_spellSlotsUsed < _spellSlotsAvailable) yield return new TemplateValidation { Level = ValidationLevel.WARNING, Message = $"Not Enough Spells: Add {_spellSlotsAvailable - _spellSlotsUsed }" };
    }
}
