// See https://aka.ms/new-console-template for more information

using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;
using FabulaUltimaDataImporter.Processor;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;

internal class SkillWorkflow : IWorkflow
{
    private readonly Instance _database;
    private readonly UserIOWrapper _userIOWrapper;
    private readonly Resolver _skillResolver;

    public SkillWorkflow(Instance db, UserIOWrapper userIOWrapper, Resolver skillResolver)
    {
        _database = db;
        _userIOWrapper = userIOWrapper;
        _skillResolver = skillResolver;
    }

    public SkillInputData? SkillInputData { private get; set; }

    public string GetName => "SkillWorkflow";

    public WorkFlowKind Kind => WorkFlowKind.SKILL;

    public IEnumerable<IWorkflow> Run()
    {
        if(SkillInputData == null)
        {
            return RunCreateSkill();
        }
        return RunBeastSkillAssignment();
    }

    private IEnumerable<IWorkflow> RunCreateSkill()
    {
        var skillName = _userIOWrapper.GetValidString("Skill Name");
        _userIOWrapper.WriteLine($"Creating new Skill {skillName}");
        var isSpecialAttack = _userIOWrapper.GetBoolean($"Is '{skillName}' a Special Attack?", "yes", "no");
        SkillTemplate skill;
        if (isSpecialAttack == true)
        {
            skill = GetSpecialAttackSkill(skillName);
        }
        else
        {
            skill = GetSkill(skillName);
        }
        var isFree = _userIOWrapper.GetBoolean($"Is '{skillName}' a Free?", "yes", "no");
        if(isFree == true) skill.OtherAttributes[KnownSkills.IS_SPECIAL_ATTACK_DETRIMENT] = true.ToString();
        _database.AddSkills(new[] {skill});
        _skillResolver.RebuildSpecialAttackIndex();
        yield break;
    }

    public IEnumerable<IWorkflow> RunBeastSkillAssignment()
    {
        var beast = _database.GetBeast(SkillInputData.BeastId) ?? throw new SkillWorkFlowException($"beast Id {SkillInputData?.BeastId} invalid");
        var beastAttacks = beast.AllAttacks.ToArray();

        foreach (var attack in beastAttacks)
        {
            _userIOWrapper.WriteLine($"Set {attack.Name} Modifiers");
            SkillInputData.AttackModifiers[attack.Id] = new AttackModifier
            {
                AtkMod = (int)_userIOWrapper.GetUnsignedInt("Accuracy Mod", 0),
                DamMod = (int)_userIOWrapper.GetUnsignedInt("Damage Mod", 0),
                Text = _userIOWrapper.GetValidString("Attack text", true),
                AttackId = attack.Id
            };
        }
       
        var skillResolution = _skillResolver.ResolveSkills(beast, SkillInputData);

        _userIOWrapper.WriteLine("Skills added");
        foreach(var pair in skillResolution.SkillSlots.Where(s => s != null))
        {
            var skill = pair.Value.skill;
            if(IsSpecialAttack(skill))
            {
                var attackId = pair.Value.targetId;
                var basicAttack = beastAttacks.FirstOrDefault(a => a.Id == attackId);
                _userIOWrapper.WriteLine($"     Special Attack Skill {skill.Name} for attack {basicAttack.Name}");
            }
            else
            {
                _userIOWrapper.WriteLine($"     Skill {skill.Name}");
            }            
        }


        var otherSkills = GetSkills(skillResolution.RemainingSkillPoints, beastAttacks).ToArray();

        var skillsToCreate = otherSkills.Select(p => p.Value.skill).Where(s =>
        {
            if(!s.OtherAttributes.TryGetValue(IS_NEW, out var value)) return false;
            if(!bool.TryParse(value, out var isNew)) return false;
            s.OtherAttributes[IS_NEW] = null; // this removes the key
            return isNew;
        }).ToArray();

        _database.AddSkills(skillsToCreate);

        var skillSlots = skillResolution.SkillSlots.Where(s => s != null).Concat(otherSkills).ToArray();        
        var beastSkillEntries = skillSlots                    
                    .Select(s => new BeastSkillEntry { BeastTemplateId = beast.Id, SkillId = s.Value.skill.Id, BasicAttackId = s.Value.targetId });        

        _database.AssignSkills(beast.Id, beastSkillEntries);
        _skillResolver.RebuildSpecialAttackIndex();
        yield break;
    }

    private const string IS_NEW = "IsNew";

    private IEnumerable<(SkillTemplate skill, Guid? targetId)?> GetSkills(int remainingSkillPoints, IReadOnlyList<BasicAttackTemplate> allAttacks)
    {
        if (remainingSkillPoints <= 0) yield break;
        var skillMap = _database.GetSkills().ToDictionary(s => s.Name.ToLowerInvariant(), s => s);


        _userIOWrapper.WriteLine($"Define/Add {remainingSkillPoints} Skills to current npc");

        foreach (var _ in Enumerable.Range(0, remainingSkillPoints))
        {
            SkillTemplate skill;
            Guid? attackId;
            GetSkill(allAttacks, skillMap, out skill, out attackId);
            yield return (skill, attackId);
        }
    }

    private void GetSkill(IReadOnlyList<BasicAttackTemplate> allAttacks, Dictionary<string, SkillTemplate> skillMap, out SkillTemplate skill, out Guid? attackId)
    {
        _userIOWrapper.WriteLine("Next Skill");
        var skillName = _userIOWrapper.GetValidString("Skill Name");
        attackId = null;
        if (skillMap.ContainsKey(skillName.ToLowerInvariant()))
        {
            _userIOWrapper.WriteLine($"Skill '{skillName}' found");
            skill = skillMap[skillName.ToLowerInvariant()];
            if (IsSpecialAttack(skill))
            {
                attackId = GetTargetAttack(allAttacks);
            }

        }
        else
        {
            _userIOWrapper.WriteLine($"Creating Skill '{skillName}'");
            var isSpecialAttack = _userIOWrapper.GetBoolean($"Is '{skillName}' a Special Attack?", "yes", "no");
            if (isSpecialAttack == true)
            {
                skill = GetSpecialAttackSkill(skillName, allAttacks, out attackId);
            }
            else
            {
                skill = GetSkill(skillName);
            }
        }
    }

    private Guid GetTargetAttack(IReadOnlyList<BasicAttackTemplate> allAttacks)
    {
        _userIOWrapper.WriteLine("Choose from the following attacks");
        _userIOWrapper.WriteLines(allAttacks.Select((a, i) => $"{i + 1}. {a.Name}").ToArray());
        var index = (int) (_userIOWrapper.GetUnsignedInt("Choice") - 1);
        return allAttacks[index].Id;
    }

    private static bool IsSpecialAttack(SkillTemplate skillTemplate)
    {
        if (!skillTemplate.OtherAttributes.TryGetValue(KnownSkills.IS_SPECIAL_ATTACK, out var isSpecialAttackString)) return false;
        if (!bool.TryParse(isSpecialAttackString, out var isSpecialAttack)) return false;
        return isSpecialAttack;
    }

    private SkillTemplate GetSkill(string skillName)
    {
        var text = _userIOWrapper.GetValidString("Skill Description");        
        var keywords = _userIOWrapper.GetValidString("Keywords (choose out of the description and separate by commas)");        
        return new SkillTemplate(Guid.NewGuid())
        {
            Name = skillName,
            TargetType = typeof(BeastTemplate),
            IsSpecialRule = true,            
            Text = text,
            Keywords = keywords.Split(",").Select(k => k.Trim().ToLowerInvariant()).ToHashSet(),    
            OtherAttributes = new SkillAttributeCollection
            {
                [IS_NEW] = true.ToString(),
                ["CreationDate"] = DateTimeOffset.UtcNow.ToString(),
            }
        };
    }

    private SkillTemplate GetSpecialAttackSkill(string skillName, IReadOnlyList<BasicAttackTemplate> allAttacks, out Guid? attackId)
    {
        attackId = GetTargetAttack(allAttacks);
        return GetSpecialAttackSkill(skillName);       
    }

    private SkillTemplate GetSpecialAttackSkill(string skillName)
    {
        var skill = GetSkill(skillName);
        skill.IsSpecialRule = false;
        skill.TargetType = typeof(BasicAttackTemplate);
        skill.OtherAttributes[KnownSkills.IS_SPECIAL_ATTACK] = true.ToString();
        return skill;
    }
}