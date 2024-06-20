using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkillList : VBoxContainer, INpcReader
{
    private NpcInstance _instance;
    private IDictionary<Guid, (SkillTemplate skill, BasicAttackTemplate attack)> _nodeIdToSkillMap = new Dictionary<Guid, (SkillTemplate skill, BasicAttackTemplate attack)>();
    private int _numSkillsToCreate;

    [Export]
	public PackedScene SkillScene { get; set; }

    [Signal]
    public delegate void SkillsDefinedEventHandler(bool defined);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        foreach (var child in this.GetChildren())
        {
            child.QueueFree();
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSkillsReady(int numSkillsToCreate)
	{
        _numSkillsToCreate = numSkillsToCreate;
        foreach (var child in this.GetChildren())
        {
            child.QueueFree();
        }
        _nodeIdToSkillMap.Clear();
        foreach (var _ in Enumerable.Range(0, numSkillsToCreate)) 
		{
			var skillScene = SkillScene.Instantiate<SkillEntry>();						
			AddChild(skillScene);
            skillScene.Update(_instance);
            skillScene.SkillDefined += OnSkillDefined;
        }
	}

    private void OnSkillDefined(Guid guid, SkillTemplate skill, BasicAttackTemplate attack)
    {
        _nodeIdToSkillMap[guid] = new(skill, attack);
        if (_nodeIdToSkillMap.Values.Count(p => p.skill != null) != _numSkillsToCreate)
        {
            EmitSignal(SignalName.SkillsDefined, false);
            return;
        }

        
        var attackIdMapToSkills = _nodeIdToSkillMap.Values
            .Where(p => p.attack != null)
            .GroupBy(p => p.attack.Id)
            .ToDictionary(g => g.Key, g => g.Select(p => p.skill));
                
        foreach (var npcAttack in _instance.Model.NpcAttacks)
        {
            // remove all rank skills first
            npcAttack.RankSkills = new Godot.Collections.Array<NpcSkill>();
        }

        if (attackIdMapToSkills.Any())
        {
            foreach (var npcAttack in _instance.Model.NpcAttacks)
            {
                if (!attackIdMapToSkills.TryGetValue(npcAttack.BasicAttackTemplate.Id, out var skillsToAdd)) continue;
                npcAttack.RankSkills = new Godot.Collections.Array<NpcSkill>(skillsToAdd.Select(s => new NpcSkill(s))); // have to replace or else the attack template won't be updated
            }
        }
        // this is to make the npc instance refresh happen after the npc attack skills are set
        _instance.Model.RankSkills = new Godot.Collections.Array<NpcSkill>(_nodeIdToSkillMap.Values.Where(p => p.attack == null).Select(p => new NpcSkill(p.skill)));

        EmitSignal(SignalName.SkillsDefined, true);
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
    }
}
