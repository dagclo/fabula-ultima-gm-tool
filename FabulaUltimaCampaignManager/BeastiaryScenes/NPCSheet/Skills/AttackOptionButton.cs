using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class AttackOptionButton : OptionButton, IValidatable
{    
    private IDictionary<int, BasicAttackTemplate> _attackMap;
    private SkillTemplate _skill;
    private BasicAttackTemplate _currentAttack;

    [Signal]
    public delegate void RemoveSkillEventHandler();

    [Signal]
    public delegate void AttackSkillSetEventHandler();

    public void HandleBeastChanged(SignalWrapper<IBeastTemplate> signal)
    {
        var beastTemplate = signal.Value;
                
        var index = 0;
        var attacks = beastTemplate.AllAttacks;
        _attackMap = _attackMap ?? new Dictionary<int, BasicAttackTemplate>();
        BasicAttackTemplate attack;
        var foundAttack = attacks.FirstOrDefault(a => a.AttackSkills.Any(s => s.Id == _skill.Id));
        if (foundAttack != null)
        {
            attack = foundAttack;
        }
        else
        {
            attack = _attackMap.TryGetValue(this.Selected, out var value) ? value : null;
        }
        
        if (_attackMap.Any())
        {
            _attackMap.Clear();
            this.Clear();
        }
        
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

        int selected = -1;
        // reselect current target if available
        if (attack != null)
        {
            var storedAttackCandidates = _attackMap.Where(p => p.Value == attack).Select(p => p.Key);
            if(storedAttackCandidates.Any())
            {
                selected = storedAttackCandidates.Single();
            }
            else
            {
                EmitSignal(SignalName.RemoveSkill);
            }
            _currentAttack = attack;
        }

        this.Selected = selected;
    }    

    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal, bool _)
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
        EmitSignal(SignalName.AttackSkillSet);
    }

    public void HandleRemoveSkill()
    {
        if( _skill == null) return;
        _currentAttack?.AttackSkills.Remove(_skill);
        EmitSignal(SignalName.RemoveSkill);
    }

    string IValidatable.Name => "Attack Skill";
    public IEnumerable<TemplateValidation> Validate()
    {
        if (_skill == null) yield break;
        if(_skill.OtherAttributes?.IsSpecialAttack != true) yield break;
        if (_currentAttack == null) yield return new TemplateValidation { Level = ValidationLevel.ERROR, Message = $"Special Attack '{_skill.Name}' not assigned to attack " };
    }
}
