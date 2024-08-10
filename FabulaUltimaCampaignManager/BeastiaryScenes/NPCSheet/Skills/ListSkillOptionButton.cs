using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject;
using Godot;
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
        var validSkillGroupedBySpecialAttack = beastRepository.Database.GetSkills()
                            .Where(s => !(s.IsSpeciesSkill() || s.IsResistanceSkill()))
                            .Where(s => s.Id != KnownSkills.UseEquipment.Id)
                            .GroupBy(s => s.OtherAttributes?.IsSpecialAttack ?? false);

        foreach (var skillGroup in validSkillGroupedBySpecialAttack)
        {
            var isSpecialAttack = skillGroup.Key;
            var text = isSpecialAttack ? "Special Attack" : "Other";
            AddItem($"===={text}====", index);
            SetItemDisabled(index, true);
            index++;

            foreach (var skill in skillGroup)
            {
                AddItem(skill.Name, index);
                _skillMap[index] = skill;
                index++;
            }            
        }
        this.Selected = -1;
    }
	
}
