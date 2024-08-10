using FabulaUltimaNpc;
using FirstProject;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SpellOptionButton : OptionButton
{
    private IDictionary<int, SpellTemplate> _spellMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;

        var index = 0;
        _spellMap = new Dictionary<int, SpellTemplate>();
        foreach (var spell in beastRepository.Database.GetSpellTemplates().OrderBy(s => s.Name))
        {
            AddItem(spell.Name, index);
            _spellMap[index] = spell;
            index++;
        }
        this.Selected = -1;
    }	
}
