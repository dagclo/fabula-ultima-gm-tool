using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using Godot;
using System;
using System.Collections.Generic;

public partial class AffinityEdit : Control, IBeastAttribute
{
    private SkilledBeastTemplateWrapper _beastTemplate;

    [Signal]
    public delegate void UpdateAffinityEventHandler(string affinityValue);

    [Signal]
    public delegate void UpdateElementEventHandler(string element);

    [Export]
    public string AffinityName { get; set; }

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(AffinityName != null) EmitSignal(SignalName.UpdateElement, AffinityName);
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        if (beastTemplate is not SkilledBeastTemplateWrapper skilledBeast) return;
        _beastTemplate = skilledBeast;
        string affinity = string.Empty;
        if(_beastTemplate.Resistances.TryGetValue(AffinityName.ToLowerInvariant(), out var resistance))
        {
            affinity = resistance.Affinity;
        }        
        EmitSignal(SignalName.UpdateAffinity, affinity);
    }

    public void HandleAffinitySelected(string affinity)
    {
        SkillTemplate skill = null;
        var skillKey = AffinityName.ToLowerInvariant();
        switch(affinity)
        {
            case "VU":
                skill = KnownSkills.VulnerabilitySkills[DamageConstants.DamageTypeMap[skillKey]]; 
                break;
            case "AB":
                skill = KnownSkills.AbsorptionSkills[DamageConstants.DamageTypeMap[skillKey]];
                break;
            case "IM":
                skill = KnownSkills.ImmunitySkills[DamageConstants.DamageTypeMap[skillKey]];
                break;
            case "RS":
                skill = KnownSkills.ResistanceSkills[DamageConstants.DamageTypeMap[skillKey]];
                break;           
        }
        var skillId = _beastTemplate.Resistances[AffinityName.ToLowerInvariant()].SkillId ?? throw new Exception("unset skill id");
        _beastTemplate.RemoveAffinitySkill(skillId);
        if (skill == null) return;
        _beastTemplate.AddSkill(skill);
    }
}
