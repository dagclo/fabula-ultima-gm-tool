using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AffinityEdit : Control, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Signal]
    public delegate void UpdateAffinityEventHandler(SignalWrapper<BeastResistance> affinityValue);

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
        _beastTemplate = beastTemplate;
        string affinity = string.Empty;
        if(_beastTemplate.Resistances.TryGetValue(AffinityName.ToLowerInvariant(), out var resistance))
        {
            affinity = resistance.Affinity;
        }        
        EmitSignal(SignalName.UpdateAffinity, new SignalWrapper<BeastResistance>(resistance));
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
        _beastTemplate.Model.RemoveSkill(skillId);
        if (skill != null) _beastTemplate.Model.AddSkill(skill);
        BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.TRIGGER }.ToHashSet());
    }
}
