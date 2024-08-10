using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AddedSkillEntry : VBoxContainer
{
    private IBeastTemplate _beastTemplate;

    [Signal]
    public delegate void SkillSetEventHandler(SignalWrapper<SkillTemplate> skill);
    public SkillTemplate Skill { get; internal set; }
	public Action<AddedSkillEntry> OnRemoveSkill { get; set; }

    public override void _Ready()
    {
        if (Skill == null) return;
        EmitSignal(SignalName.SkillSet, new SignalWrapper<SkillTemplate>(Skill));
    }

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }
}
