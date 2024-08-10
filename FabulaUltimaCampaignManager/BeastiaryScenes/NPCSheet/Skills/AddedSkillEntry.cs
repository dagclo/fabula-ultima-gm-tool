using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AddedSkillEntry : VBoxContainer
{  

    [Signal]
    public delegate void SkillSetEventHandler(SignalWrapper<SkillTemplate> skill);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> skill);
    public SkillTemplate Skill { get; internal set; }
	public Action<AddedSkillEntry> OnRemoveSkill { get; set; }
    public Action OnUpdateBeast { get; set; }

    public override void _Ready()
    {
        if (Skill == null) return;
        EmitSignal(SignalName.SkillSet, new SignalWrapper<SkillTemplate>(Skill));
    }

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {        
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveSkill()
    {
        OnRemoveSkill?.Invoke(this);        
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }
}
