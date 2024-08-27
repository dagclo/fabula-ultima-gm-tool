using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleStatus : Resource
{
    private readonly Guid _instanceId;
    private IDictionary<StatusEffect, bool> _effectOn = new Dictionary<StatusEffect, bool>();

    public BattleStatus(NpcInstance c)
    {
        _currentHP = c.Template.HealthPoints;
        _currentMP = c.Template.MagicPoints;
        _instanceId = Guid.NewGuid();
        _maxTurns = c.Model.Rank.GetNumSoldiersReplaced();
        _numTurnsLeft = _maxTurns;
    }

    public BattleStatus()
    {
    }

    public Action<BattleStatus> StatusChanged { get; set; }
    public Action<BattleStatus> StudyLevelChanged { get; set; }

    private int _numTurnsLeft;
    public int NumTurnsLeft
    {
        get => _numTurnsLeft;
        private set
        {
            if (_numTurnsLeft != value)
            {
                _numTurnsLeft = value;
                StatusChanged.Invoke(this);
            }
        }
    }
    private readonly int _maxTurns;
    public void ResetTurns()
    {
        NumTurnsLeft = _maxTurns;
    }

    public void DecrementTurns()
    {
        if (NumTurnsLeft == 0) return;
        NumTurnsLeft--;
    }

    private bool _isActive = true;
    public bool IsActive => NumTurnsLeft > 0;
    public void Kill() => CurrentHP = 0;

    public bool IsDead => CurrentHP <= 0;

    private int _currentHP;
    public int CurrentHP 
    {
        get => _currentHP;
        set
        {
            if (_currentHP != value) 
            { 
                _currentHP = value;
                StatusChanged?.Invoke(this);
            }
        }
    }
    private int _currentMP;
    public int CurrentMP
    {
        get => _currentMP;
        set
        {
            if (_currentMP != value)
            {
                _currentMP = value;
                StatusChanged?.Invoke(this);
            }
        }
    }

    private StudyLevelEnum _currentStudyLevel;
    public StudyLevelEnum StudyLevel
    {
        get => _currentStudyLevel;
        set
        {
            if (_currentStudyLevel != value)
            {
                _currentStudyLevel = value;
                StudyLevelChanged?.Invoke(this);
            }
        }
    }

    public bool Alive { get; internal set; } = true;

    public bool IsStatusInEffect(StatusEffect status)
    {
        if(_effectOn.TryGetValue(status, out var isOn))
        {
            return isOn;
        }
        return false;
    }

    public void UpdateStatusEffect(StatusEffect status, bool value)
    {
        if (_effectOn.TryGetValue(status, out var curVal) && value == curVal) return;
        _effectOn[status] = value;
        StatusChanged?.Invoke(this);
    }

    public override string ToString()
    {
        return _instanceId.ToString();
    }

    private readonly IDictionary<string, ISet<StatusEffect>> _attributesToStatusEffect = new Dictionary<string, ISet<StatusEffect>>()
    {
        { nameof(IBeastTemplate.Insight), new HashSet<StatusEffect>() { StatusEffect.DAZED, StatusEffect.ENRAGED } },
        { nameof(IBeastTemplate.Might), new HashSet<StatusEffect>() { StatusEffect.WEAK, StatusEffect.POISONED } },
        { nameof(IBeastTemplate.Dexterity), new HashSet<StatusEffect>() { StatusEffect.ENRAGED, StatusEffect.SLOW } },
        { nameof(IBeastTemplate.WillPower), new HashSet<StatusEffect>() { StatusEffect.SHAKEN, StatusEffect.POISONED } },
    };

    internal Die ApplyStatus(string attributeName, Die attributeDie)
    {
        Die result = attributeDie;
        foreach(var effect in _attributesToStatusEffect[attributeName].Where(s => IsStatusInEffect(s)))
        {
            result = Die.Downgrade(attributeDie);
        }
        return result;
    }

    internal int ApplyStatus(string attribute, NpcInstance npcInstance)
    {
        int result;
        switch (attribute)
        {
            case "PDef":
                if (npcInstance.Template.HasDefenseOverride) // overrides are unaffected by status
                {
                    result = npcInstance.Template.Defense;
                }
                else
                {
                    var postStatusDexDie = ApplyStatus(nameof(IBeastTemplate.Dexterity), npcInstance.Template.Dexterity);
                    result = npcInstance.Template.Defense + (postStatusDexDie.Sides - npcInstance.Template.Dexterity.Sides);
                }                
                break;
            case "MDef":
                var postIStatusInsightDie = ApplyStatus(nameof(IBeastTemplate.Insight), npcInstance.Template.Insight);
                result = npcInstance.Template.MagicalDefense + (postIStatusInsightDie.Sides - npcInstance.Template.Insight.Sides);
                break;
            default:
                result = 0;
                break;
        }
        return result;
    }

    public enum StudyLevelEnum
    {
        NO_INFO = 1,
        SOME_INFO,
        MOST_INFO,
        ALL_INFO,
    }
}
