using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpellOptionButton : OptionButton
{
    private IDictionary<int, SpellTemplate> _spellMap;

    [Signal]
    public delegate void SpellSelectedEventHandler(SignalWrapper<SpellTemplate> equipment);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;

        var id = 0;
        AddItem("Create New Spell", id++);
        _spellMap = new Dictionary<int, SpellTemplate>();
        foreach (var spell in beastRepository.Database.GetSpellTemplates().OrderBy(s => s.Name))
        {
            AddItem(spell.Name, id);
            var curIndex = GetItemIndex(id);
            SetItemTooltip(curIndex, string.IsNullOrEmpty(spell.Description) ? spell.Name : spell.Description); // attempt to fix off by 1 tooltips
            _spellMap[id] = spell;
            id++;
        }
        this.Selected = -1;
    }

    public void HandleSpellSelected(int index)
    {
        SpellTemplate spell;
        if (index == 0)
        {
            spell = new SpellTemplate()
            {
                Id = Guid.NewGuid(),
            };
        }
        else
        {
            spell = _spellMap[index];
        }
        EmitSignal(SignalName.SpellSelected, new SignalWrapper<SpellTemplate>(spell));
    }

    public void HandleSpellAdded()
    {
        this.Selected = -1;
    }
}
