using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AttackOptionButton : OptionButton
{    
    private IDictionary<int, BasicAttackTemplate> _attackMap;
    private SkillTemplate _skill;
    private BasicAttackTemplate _currentAttack;

    public void HandleBeastChanged(SignalWrapper<IBeastTemplate> signal)
    {
        var beastTemplate = signal.Value;
        var index = 0;
        var attacks = beastTemplate.AllAttacks;
        _attackMap = _attackMap ?? new Dictionary<int, BasicAttackTemplate>();
        _attackMap.Clear();
        var attacksGroupedByType = attacks.GroupBy(a => a.IsEquipmentAttack);
        // clear options
        foreach (var attackGroup in attacksGroupedByType)
        {
            var isEquipmentAttack = attackGroup.Key;
            var text = isEquipmentAttack ? "Equipment" : "Basic";
            AddItem($"===={text}====", index);
            SetItemDisabled(index, true);
            index++;

            foreach (var skill in attackGroup.OrderBy(s => s.Name))
            {
                AddItem(skill.Name, index);
                _attackMap[index] = skill;
                index++;
            }
        }
        // set to current option
        this.Selected = -1;
    }

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal)
    {
        _skill = signal.Value;
    }

    public void HandleAttackSelected(int index)
    {        
        if(_skill == null) return;
        var attack = _attackMap[index];
        if(_currentAttack != null)
        {
            _currentAttack.AttackSkills.Remove(_skill);
        }
        _currentAttack = attack;
        _currentAttack.AttackSkills.Add(_skill);
    }
}
