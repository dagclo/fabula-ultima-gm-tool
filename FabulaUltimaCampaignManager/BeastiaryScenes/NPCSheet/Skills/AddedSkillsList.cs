using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AddedSkillsList : VBoxContainer, IBeastAttribute
{
    private IBeast _beastModel;
    private SkillTemplate _nextSkillToAdd;
    private IBeastTemplate _beastTemplate;

    private Action<IBeastTemplate> OnBeastChanged { get; set; }

    [Export]
    public PackedScene AddSkillScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    private bool _addedExisting = false;

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastModel = beastTemplate.Model;
        OnBeastChanged?.Invoke(beastTemplate);
        _beastTemplate = beastTemplate;
        if (_addedExisting) return;        
        foreach(var skill in _beastModel.Skills.Where(s => !(s.IsSpeciesSkill() || s.IsAffinitySkill()))
                            .Where(s => s.Id != KnownSkills.UseEquipment.Id))
        {
            AddSkill(skill);
        }
        _addedExisting = true;
    }

    public void HandleSkillSelect(SignalWrapper<SkillTemplate> signalWrapper)
    {
        _nextSkillToAdd = signalWrapper.Value;
    }

    public void HandleAddSkill()
    {
        if (_nextSkillToAdd == null) return;
        var skill = _nextSkillToAdd;
        _beastModel.AddSkill(skill);
        AddSkill(skill);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }

    private void AddSkill(SkillTemplate skill)
    {
        var scene = AddSkillScene.Instantiate<AddedSkillEntry>();
        scene.Skill = skill;
        scene.OnRemoveSkill += HandleRemoveSkill;
        scene.OnUpdateBeast += HandleUpdateBeast;
        OnBeastChanged += scene.HandleBeastChanged;
        AddChild(scene);
        scene.HandleBeastChanged(_beastTemplate);
    }

    private void HandleUpdateBeast()
    {
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }

    private void HandleRemoveSkill(AddedSkillEntry entry)
    {
        _beastModel.RemoveSkill(entry.Skill);
        OnBeastChanged -= entry.HandleBeastChanged;
        RemoveChild(entry);
        entry.QueueFree();
        OnBeastChanged?.Invoke(_beastTemplate);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }    
}
