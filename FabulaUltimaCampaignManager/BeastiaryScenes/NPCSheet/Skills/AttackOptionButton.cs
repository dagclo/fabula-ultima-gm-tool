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

    public void HandleBeastChanged(SignalWrapper<IBeastTemplate> signal)
    {
        var beastTemplate = signal.Value;
        var index = 0;
        var attacks = beastTemplate.AllAttacks;
        _attackMap = _attackMap ?? new Dictionary<int, BasicAttackTemplate>();
        _attackMap.Clear();
        var attacksGroupedByType = attacks.GroupBy(a => a.IsEquipmentAttack);

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
        this.Selected = -1;
    }
}
