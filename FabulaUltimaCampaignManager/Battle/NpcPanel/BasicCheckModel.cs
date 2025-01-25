using FabulaUltimaNpc;
using System;

public class BasicCheckModel : ICheckModel
{    
    public string Action { get; set; }

    private Die _attribute1die;
    public Die Attribute1Die
    {
        get
        {
            return _attribute1die;
        }
        set
        {
            _attribute1die = value;
            Changed?.Invoke();
        }
    }

    private string _attribute1Name;
    public string Attribute1Name
    {
        get
        {
            return _attribute1Name;
        }
        set
        {
            _attribute1Name = value;
            Changed?.Invoke();
        }
    }

    private Die _attribute2die;
    public Die Attribute2Die
    {
        get
        {
            return _attribute2die;
        }
        set
        {
            _attribute2die = value;
            Changed?.Invoke();
        }
    }

    private string _attribute2Name;
    public string Attribute2Name
    {
        get
        {
            return _attribute2Name;
        }
        set
        {
            _attribute2Name = value;
            Changed?.Invoke();
        }
    }

    public int? AccuracyMod { get; set; }    

    private int _difficulty;
    public int Difficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
            Changed?.Invoke();
        }
    }
    public int? HighRollMod { get; set; }
    public string DamageType { get; set; }
    public Action Changed { get; set; }

    public bool IsValid =>
        Difficulty > 0
        && !string.IsNullOrWhiteSpace(Attribute1Name)
        && !string.IsNullOrWhiteSpace(Attribute2Name)
        &&(!string.IsNullOrWhiteSpace(Target) || !TargetRequired); // only care if target required and no target
    public bool TargetRequired { private get; set; }

    private string _target;
    public string Target
    {
        get => _target;
        set
        {
            _target = value;
            Changed?.Invoke();
        }
    }
}
