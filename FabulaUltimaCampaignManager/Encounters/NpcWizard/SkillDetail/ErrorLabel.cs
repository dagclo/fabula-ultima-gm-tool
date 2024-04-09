using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using FabulaUltimaSkillLibrary;

public partial class ErrorLabel : Label, ISkillReader, IHasId
{
    public Guid Id { private get; set; }

    [Signal]
	public delegate void SkillReadyEventHandler(SignalWrapper<Guid> id, SignalWrapper<SkillTemplate> skill, SignalWrapper<BasicAttackTemplate> attack);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

    public void HandleSkillChanged(SkillTemplate skill)
    {
        this.Visible = false;
        if (skill.ModifiesAttack()) return;
        // for now this handles non special attacks
        if (IsValid(skill, out var error))
		{
            
            EmitSignal(SignalName.SkillReady, new SignalWrapper<Guid>(Id), new SignalWrapper<SkillTemplate>(skill), new SignalWrapper<BasicAttackTemplate>(null));
			return;
        }
        this.Visible = true;
        this.Text = error;
        EmitSignal(SignalName.SkillReady, new SignalWrapper<Guid>(Id), new SignalWrapper<SkillTemplate>(null), new SignalWrapper<BasicAttackTemplate>(null));
    }

    private bool IsValid(SkillTemplate skill, out string error)
    {
		// update this with 
		if (skill.IsSpellcasterSkill())
		{
			error = "Spellcaster skills not supported...yet";            
            return false;
		}
		if (skill.IsResistanceSkill())
		{
            error = "Resistance skills not supported...yet";
            return false;
		}
		if(KnownSkills.SpecializedMagicCheck.Id == skill.Id)
		{
            error = "Specialised magic check skills not supported...yet";
            return false;
        }
		error = string.Empty;
		return true;
    }
}
