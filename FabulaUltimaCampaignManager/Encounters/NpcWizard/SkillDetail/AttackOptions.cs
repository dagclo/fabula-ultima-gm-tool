using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using FabulaUltimaSkillLibrary;

public partial class AttackOptions : OptionButton, ISkillReader, INpcReader, IHasId
{
    private NpcInstance _instance;
    private SkillTemplate _skill;
    private List<BasicAttackTemplate> _attacks;
    
    public Guid Id { private get; set; }

    [Signal]
    public delegate void SkillReadyEventHandler(SignalWrapper<Guid> id, SignalWrapper<SkillTemplate> skill, SignalWrapper<BasicAttackTemplate> attack);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Id = Guid.NewGuid();
    }

    public void HandleSkillChanged(SkillTemplate skill)
    {
        _skill = skill;
        this.Clear();                
        if (_instance == null) return;
        if (!skill.ModifiesAttack()) return;
        _attacks = _instance.Template.AllAttacks.ToList();
        foreach ((var attack, var index) in _attacks.Select((a, i) => (a, i)))
        {
            AddItem(attack.Name, index);
        }
        this.Selected = -1;
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
    }

    public void OnItemSelected(int index)
    {
        var id = GetItemId(index);
        var attack = _attacks[id];
        EmitSignal(SignalName.SkillReady, new SignalWrapper<Guid>(Id), new SignalWrapper<SkillTemplate>(_skill), new SignalWrapper<BasicAttackTemplate>(attack));
    }
}
