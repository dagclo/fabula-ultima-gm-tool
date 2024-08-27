using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpeciesSelect : OptionButton, IBeastAttribute, IValidatable
{
    [Signal]
    public delegate void SpeciesUpdateEncounterEventHandler(SignalWrapper<IEnumerable<SkillTemplate>> speciesSkills);
    private IBeastTemplate _beastTemplate;
    private BeastiaryRepository _beastRepository;
    private Dictionary<string, int> _speciesIndexMap;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        var speciesList = new[] { "Beast", "Construct", "Demon", "Elemental", "Humanoid", "Monster", "Plant", "Undead" };
        _speciesIndexMap = speciesList.Select((s, i) => (s, i)).ToDictionary(p => p.s, p => p.i);
        foreach (var val in speciesList)
        {
            AddItem(val);
        }
        this.Selected = -1;
    }

    public void HandleItemSelect(int index)
    {
        if (_beastTemplate == null) return;
        GetSpecies(index);
    }

    private void GetSpecies(int index)
    {
        var species = GetItemText(index);
        _beastTemplate.Species = new SpeciesType(SpeciesConstants.FromString(species), species);
        BeastTemplateAction.Invoke(new HashSet<BeastEntryNode.Action>() { BeastEntryNode.Action.TRIGGER });
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        if (_beastTemplate.Species != null)
        {
           this.Selected = _speciesIndexMap[_beastTemplate.Species.Name];
        }
    }
    string IValidatable.Name => "Species";
    public IEnumerable<TemplateValidation> Validate()
    {
        if (_beastTemplate.Species == null)
        {
            yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Not Set" };
            yield break;
        }
        var skills = _beastTemplate.Skills;
        var numFreeResistances = _beastRepository.Database.GetNumFreeResistances(_beastTemplate.Species);
        var numResistanceSkills = skills.Count(s => s.IsResistanceSkill());
        if(numFreeResistances > numResistanceSkills) yield return new TemplateValidation { Level = ValidationLevel.WARNING, Message = $"Species {_beastTemplate.Species.Name} Allows {numFreeResistances - numResistanceSkills} more resistances" };
        
        var numFreeImmunities = _beastRepository.Database.GetNumFreeImmunities(_beastTemplate.Species);        
        var numImmunitySkills = skills.Count(s => s.IsImmunitySkill() && !s.IsFreeSkillForSpecies(_beastTemplate.Species));
        if (numFreeImmunities > numImmunitySkills) yield return new TemplateValidation { Level = ValidationLevel.WARNING, Message = $"Species {_beastTemplate.Species.Name} Allows {numFreeImmunities - numImmunitySkills} more immunities" };

        var builtInVulnerabilityChoices = _beastRepository.Database.GetBuiltInVulnerbilityChoices(_beastTemplate.Species).VulnerabilityChoices
            .ToDictionary(c => KnownSkills.VulnerabilitySkills[c.DamageTypeId].Id, c => KnownSkills.VulnerabilitySkills[c.DamageTypeId]);
        if (builtInVulnerabilityChoices.Any() && !_beastTemplate.Skills.Any(s => builtInVulnerabilityChoices.ContainsKey(s.Id))) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Species {_beastTemplate.Species.Name} Requires one of the following:  {string.Join(",", builtInVulnerabilityChoices.Values.Select(s => s.Name))}" };
    }
}
