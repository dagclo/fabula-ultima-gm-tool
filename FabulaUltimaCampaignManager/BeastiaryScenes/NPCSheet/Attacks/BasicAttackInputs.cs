using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BasicAttackInputs : VBoxContainer, IBeastAttribute
{
    private ICollection<BasicAttackTemplate> _basicAttacks;

    [Export]
    public PackedScene BasicAttackInputScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
    public Action OnBeastUpdate { get; private set; }

    private bool _addedExisting = false;
    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _basicAttacks = beastTemplate.Model.BasicAttacks;
        if (!_addedExisting)
        {
            foreach (var attack in _basicAttacks)
            {
                AddAttack(attack);
            }
            _addedExisting = true;
        }        
        
        OnBeastUpdate?.Invoke();
    }

    public void HandleAddAttack()
    {
        if (_basicAttacks == null) return;
        var newAttack = new BasicAttackTemplate
        {
            Id = Guid.NewGuid(),
            DamageMod = 5,
            AccuracyMod = 0,
            AttackSkills = new List<SkillTemplate>()
        };
        _basicAttacks.Add(newAttack);
        AddAttack(newAttack);
        BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
    }

    private void AddAttack(BasicAttackTemplate newAttack)
    {
        var scene = BasicAttackInputScene.Instantiate<BasicAttackSettings>();        
        scene.BasicAttack = newAttack;
        scene.OnRemoveAttack += HandleAttackRemove;
        OnBeastUpdate += scene.HandleBeastUpdate;
        this.AddChild(scene);
    }

    private void HandleAttackRemove(BasicAttackSettings scene)
    {
        var attack = scene.BasicAttack;
        _basicAttacks.Remove(attack);
        OnBeastUpdate -= scene.HandleBeastUpdate;
        RemoveChild(scene);
        scene.QueueFree();
        if (attack.AttackSkills.Any())
        {
            BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.TRIGGER }.ToHashSet());
        }
        else
        {
            BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
        }
        
    }
}
