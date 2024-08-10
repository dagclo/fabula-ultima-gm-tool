using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpellCount : Label, IBeastAttribute
{
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Text = "0/0";
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var spellSkills = beastTemplate.Skills.Where(s => s.IsSpellcasterSkill());
        var spellSlotsAvailable = spellSkills.Select(s => int.Parse(s.OtherAttributes[StatsConstants.NUM_SPELLS])).Sum();
        var spellSlotsUsed = beastTemplate.Spells.Count();
        this.Text = $"{spellSlotsUsed}/{spellSlotsAvailable}";
    }
}
