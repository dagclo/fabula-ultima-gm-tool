using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ListSkillOptionButton : OptionButton
{
    private IDictionary<int, SkillTemplate> _skillMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;

        var index = 0;
        _skillMap = new Dictionary<int, SkillTemplate>();
        var validSkills = beastRepository.Database.GetSkills()
                            .Where(s => !(s.IsSpeciesSkill() || s.IsResistanceSkill()))
                            .OrderBy(s => s.Name);

        foreach (var skill in validSkills)
        {
            AddItem(skill.Name, index);
            _skillMap[index] = skill;
            index++;
        }
        this.Selected = -1;
    }
	
}
