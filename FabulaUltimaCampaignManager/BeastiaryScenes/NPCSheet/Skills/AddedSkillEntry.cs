using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddedSkillEntry : VBoxContainer, IValidatable
{
    private BeastiaryRepository _beastRepository;
    private IBeastTemplate _beastTemplate;

    [Signal]
    public delegate void SkillSetEventHandler(SignalWrapper<SkillTemplate> skill, bool editable);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> beast);
    public SkillTemplate Skill { get; internal set; }
    private bool IsEditable => Skill.Name == null;
    public Action<AddedSkillEntry> OnRemoveSkill { get; set; }
    public Action OnUpdateBeast { get; set; }

    string IValidatable.Name => "Added Skill";

    public override void _Ready()
    {
        if (Skill == null) return;
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;        
        if (IsEditable)
        {
            _beastRepository.QueueUpdates(_beastTemplate.Id, Skill);
        }
        EmitSignal(SignalName.SkillSet, new SignalWrapper<SkillTemplate>(Skill), IsEditable);
    }

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveSkill()
    {
        if (Skill == null) return;
        if (IsEditable)
        {
            _beastRepository.DequeueUpdate(_beastTemplate.Id, Skill.Id);
        }
        OnRemoveSkill?.Invoke(this);
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }

    public IEnumerable<TemplateValidation> Validate()
    {
        if (string.IsNullOrWhiteSpace(Skill.Name)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Name Not Set" };
        if (string.IsNullOrWhiteSpace(Skill.Text)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Description Not Set" };
    }
}
