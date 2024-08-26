using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CurrentSpellList : VBoxContainer, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public PackedScene AddSpellScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
    public Action<IBeastTemplate> OnBeastChanged { get; private set; }

    private bool _addedExisting = false;
    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        OnBeastChanged?.Invoke(beastTemplate);
        if (_addedExisting) return;
        foreach(var spell in _beastTemplate.Model.Spells)
        {
            AddSpell(spell);
        }
        _addedExisting = true;
    }

    public void HandleAddSpell(SignalWrapper<SpellTemplate> signal)
    {
        var spell = signal.Value;
        _beastTemplate.Model.Spells.Add(spell);
        AddSpell(spell);
        BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
    }

    private void AddSpell(SpellTemplate spell)
    {
        var scene = AddSpellScene.Instantiate<AddedSpellEntry>();
        scene.Spell = spell;
        scene.OnRemoveSpell += HandleRemoveSpell;
        scene.OnUpdateBeast += HandleUpdateBeast;
        OnBeastChanged += scene.HandleBeastChanged;
        AddChild(scene);
        scene.HandleBeastChanged(_beastTemplate);
    }

    private void HandleRemoveSpell(AddedSpellEntry entry)
    {
        _beastTemplate.Model.Spells.Remove(entry.Spell);
        OnBeastChanged -= entry.HandleBeastChanged;
        RemoveChild(entry);
        entry.QueueFree();
        OnBeastChanged?.Invoke(_beastTemplate);
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.CHANGED }));
    }

    private void HandleUpdateBeast()
    {
        BeastTemplateAction?.Invoke(new HashSet<BeastEntryNode.Action>(new[] { BeastEntryNode.Action.CHANGED }));
    }
}
