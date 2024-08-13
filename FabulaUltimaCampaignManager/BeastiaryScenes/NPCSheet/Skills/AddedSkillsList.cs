using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddedSkillsList : VBoxContainer, IBeastAttribute
{
    private ICollection<SkillTemplate> _skillsList;
    private SkillTemplate _nextSkillToAdd;
    private IBeastTemplate _beastTemplate;

    private Action<IBeastTemplate> OnBeastChanged { get; set; }

    [Export]
    public PackedScene AddSkillScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _skillsList = beastTemplate.Model.Skills;
        OnBeastChanged?.Invoke(beastTemplate);
        _beastTemplate = beastTemplate;
    }

    public void HandleSkillSelect(SignalWrapper<SkillTemplate> signalWrapper)
    {
        _nextSkillToAdd = signalWrapper.Value;
    }

    public void HandleAddSkill()
    {
        if (_nextSkillToAdd == null) return;
        var skillClone = _nextSkillToAdd; //todo: check if clone required
        _skillsList.Add(skillClone);
        var scene = AddSkillScene.Instantiate<AddedSkillEntry>();
        scene.Skill = skillClone;
        scene.OnRemoveSkill += HandleRemoveSkill;
        scene.OnUpdateBeast += HandleUpdateBeast;
        OnBeastChanged += scene.HandleBeastChanged;        
        AddChild(scene);
        scene.HandleBeastChanged(_beastTemplate);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }

    private void HandleUpdateBeast()
    {
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }

    private void HandleRemoveSkill(AddedSkillEntry entry)
    {
        _skillsList.Remove(entry.Skill);
        OnBeastChanged -= entry.HandleBeastChanged;
        RemoveChild(entry);
        entry.QueueFree();
        OnBeastChanged?.Invoke(_beastTemplate);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.TRIGGER }));
    }    
}
