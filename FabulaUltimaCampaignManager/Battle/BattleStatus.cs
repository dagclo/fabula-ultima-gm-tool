﻿using FabulaUltimaNpc;
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
    }

    public BattleStatus()
    {
    }

    public Action<BattleStatus> StatusChanged { get; set; }
    public Action<BattleStatus> StudyLevelChanged { get; set; }
    public bool IsFirst { get; set; } = false;
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

    public enum StudyLevelEnum
    {
        NO_INFO = 1,
        SOME_INFO,
        MOST_INFO,
        ALL_INFO,
    }
}
