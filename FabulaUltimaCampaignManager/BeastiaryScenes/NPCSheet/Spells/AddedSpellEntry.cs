using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AddedSpellEntry : VBoxContainer, IValidatable
{
    [Signal]
    public delegate void SpellSetEventHandler(SignalWrapper<SpellTemplate> spell, bool editable);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> beast);
    public SpellTemplate Spell { get; internal set; }

    private BeastiaryRepository _beastRepository;
    private IBeastTemplate _beastTemplate;

    private bool IsEditable { get; set; }
    public Action<AddedSpellEntry> OnRemoveSpell { get; internal set; }
    public Action OnUpdateBeast { get; internal set; }

    string IValidatable.Name => "Added Spell";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (Spell == null) return;
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        IsEditable = Spell.Name == null;
        EmitSignal(SignalName.SpellSet, new SignalWrapper<SpellTemplate>(Spell), IsEditable);
	}


    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (Spell == null) return;
        _beastTemplate = beastTemplate;
        var spellList = _beastTemplate.Model.Spells;
        bool allowUpdate = true;
        if (!_beastTemplate.Skills.Any(s => s.IsSpellcasterSkill()))
        {
            _beastTemplate.Model.RemoveSpell(Spell);
            allowUpdate = false;
        }   

        if(allowUpdate && IsEditable)
        {
            _beastRepository.QueueUpdates(_beastTemplate.Id, Spell);
        }

        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(_beastTemplate));
    }

    public void HandleRemoveSpell()
    {
        if (Spell == null) return;
        if (IsEditable)
        {
            _beastRepository.DequeueUpdate(_beastTemplate.Id, Spell.Id);
        }
        OnRemoveSpell?.Invoke(this);
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }

    public IEnumerable<TemplateValidation> Validate()
    {
        if (Spell == null || !IsEditable) yield break;
        if (string.IsNullOrWhiteSpace(Spell.Name)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Name Not Set" };
        if (string.IsNullOrWhiteSpace(Spell.Description)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Description Not Set" };
        if (string.IsNullOrWhiteSpace(Spell.Duration)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Duration Not Set" };
        if (string.IsNullOrWhiteSpace(Spell.Target)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Target Not Set" };
        if (Spell.MagicPointCost <= 0) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "MP Cost must be greater than 0" };
        if (Spell.IsOffensive)
        {
            if (string.IsNullOrWhiteSpace(Spell.Attribute1)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Attribute 1 not set. Required for Offensive Spells." };
            if (string.IsNullOrWhiteSpace(Spell.Attribute2)) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = "Attribute 2 not set. Required for Offensive Spells." };
        }
    }
}
