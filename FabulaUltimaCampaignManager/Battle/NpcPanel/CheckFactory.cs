using FabulaUltimaNpc;

using System;
using System.Collections.Generic;

public class CheckFactory
{
	private readonly IDictionary<string, Func<string, string, ICheckModel>> _factoryMap;
    private readonly IBeastTemplate _beast;
    private readonly BattleStatus _status;

    public CheckFactory(IBeastTemplate beast, BattleStatus status, bool targetRequired = false) 
	{
		_beast = beast;
		_status = status;
		_factoryMap = new Dictionary<string, Func<string, string, ICheckModel>>()
		{
			["Objective"] = new Func<string, string, ICheckModel>( (string attr1, string attr2) => 
                GenerateBasicCheck("Objective", attr1 ?? nameof(IBeastTemplate.Insight), attr2 ?? nameof(IBeastTemplate.Insight), targetRequired)),
            ["Study"] = new Func<string, string, ICheckModel>((string attr1, string attr2) => 
                GenerateBasicCheck("Study", attr1 ?? nameof(IBeastTemplate.Insight), attr2 ?? nameof(IBeastTemplate.Insight), targetRequired)),
            ["Skill"] = new Func<string, string, ICheckModel>((string attr1, string attr2) => 
                GenerateBasicCheck("Skill", attr1 ?? nameof(IBeastTemplate.Dexterity), attr2 ?? nameof(IBeastTemplate.Insight), targetRequired)),
            ["Hinder"] = new Func<string, string, ICheckModel>((string attr1, string attr2) => 
                GenerateBasicCheck("Hinder", attr1 ?? nameof(IBeastTemplate.Might), attr2 ?? nameof(IBeastTemplate.Insight), targetRequired)),
            ["Attack"] = new Func<string, string, ICheckModel>((string attr1, string attr2) =>
                GenerateBasicCheck("Attack", attr1 ?? nameof(IBeastTemplate.Might), attr2 ?? nameof(IBeastTemplate.Dexterity), targetRequired, highRollMod: 5)), // todo: generate more actions using spells and attacks
            ["Spell"] = new Func<string, string, ICheckModel>((string attr1, string attr2) =>
                GenerateBasicCheck("Spell", attr1 ?? nameof(IBeastTemplate.Insight), attr2 ?? nameof(IBeastTemplate.WillPower), targetRequired, highRollMod: 5)),
        };
    }

    private ICheckModel GenerateBasicCheck(string action, string attributeName1, string attributeName2, bool targetRequired, int? accuracyMod = null, int? highRollMod = null)
    {
        var attributeDie1 = _beast.GetDie(attributeName1);
        var attributeDie2 = _beast.GetDie(attributeName2);
		var downGradedDie1 = _status.ApplyStatus(attributeName1, attributeDie1);
        var downGradedDie2 = _status.ApplyStatus(attributeName2, attributeDie2);

		return new BasicCheckModel
		{
			Difficulty = 10, // default difficulty
			Action = action,
			Attribute1Die = downGradedDie1,
            Attribute2Die = downGradedDie2,
            Attribute1Name = attributeName1,
            Attribute2Name = attributeName2,
            AccuracyMod = accuracyMod,
            HighRollMod = highRollMod,
            TargetRequired = targetRequired
        };
    }

    public ICheckModel GetCheck(string action, string attribute1 = null, string attribute2 = null)
	{
		return _factoryMap[action](attribute1, attribute2);
	}

    internal IEnumerable<string> GetAvailableChecks() => _factoryMap.Keys;

    public IEnumerable<string> CheckTypes;
}
