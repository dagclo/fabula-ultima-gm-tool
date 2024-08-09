using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class BasicAttackInputs : VBoxContainer, IBeastAttribute
{
    private ICollection<BasicAttackTemplate> _basicAttacks;

    [Export]
    public PackedScene BasicAttackInputScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
    public Action OnBeastUpdate { get; private set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _basicAttacks = beastTemplate.Model.BasicAttacks;
        OnBeastUpdate?.Invoke();
    }

    public void HandleAddAttack()
    {
        var scene = BasicAttackInputScene.Instantiate<BasicAttackSettings>();
        var newAttack = new BasicAttackTemplate
        {
            Id = Guid.NewGuid(),
        };
        scene.BasicAttack = newAttack;
        scene.OnRemoveAttack += HandleAttackRemove;
        OnBeastUpdate += scene.HandleBeastUpdate;
        this.AddChild(scene);
    }

    private void HandleAttackRemove(BasicAttackSettings settings)
    {
        _basicAttacks.Remove(settings.BasicAttack);
        RemoveChild(settings);
        settings.QueueFree();
    }
}
