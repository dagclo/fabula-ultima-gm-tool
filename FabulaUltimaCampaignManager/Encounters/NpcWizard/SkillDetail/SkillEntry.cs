using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class SkillEntry : VBoxContainer
{
    private NpcInstance _instance;
    
    public Action<Guid, SkillTemplate, BasicAttackTemplate> SkillDefined;
    private Guid _id;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _id = Guid.NewGuid();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void Update(NpcInstance instance)
    {
        _instance = instance;
        foreach(var child in this.FindChildren("*").Where(c => c is INpcReader))
        {
            var npcReader = child as INpcReader;
            npcReader.HandleNpcChanged(_instance);            
        }

        foreach (var child in this.FindChildren("*").Where(c => c is IHasId))
        {
            var idHaver = child as IHasId;
            idHaver.Id = _id;
        }
    }

    public void OnSkillSelected(SignalWrapper<SkillTemplate> signal)
    {
        var skill = signal.Value;
        foreach (var child in this.FindChildren("*").Where(c => c is ISkillReader))
        {
            var npcReader = child as ISkillReader;
            npcReader.HandleSkillChanged(skill);
        }
    }

    public void OnSkillReady(SignalWrapper<Guid> id, SignalWrapper<SkillTemplate> skill, SignalWrapper<BasicAttackTemplate> attack)
    {
        SkillDefined?.Invoke(id.Value, skill.Value, attack.Value);
    }
}
