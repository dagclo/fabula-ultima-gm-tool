﻿using FabulaUltimaNpc;
using System.Text;

namespace FabulaUltimaSkillLibrary.Models
{
    public class SkilledBeastTemplateWrapper : IBeastTemplate
    {
        private readonly IBeastTemplate _beastTemplate;
        private IReadOnlyDictionary<Guid, int> _skillCountMap;
        private IReadOnlyDictionary<Guid, SkillTemplate> _skillMap;        

        public SkilledBeastTemplateWrapper(IBeastTemplate beastTemplate)
        {
            _beastTemplate = beastTemplate;
            UpdateSkills();
        }

        public IBeastTemplate Internal => _beastTemplate;

        public IReadOnlyCollection<ActionTemplate> Actions => _beastTemplate.Actions;

        public IEnumerable<BasicAttackTemplate> AllAttacks => ResolveAttacks(this._beastTemplate.AllAttacks).ToArray();

        private IEnumerable<BasicAttackTemplate> ResolveAttacks(IEnumerable<BasicAttackTemplate> attacks)
        {
            int accuracyMod = LevelAccuracyModifier;
            if(_skillMap.TryGetValue(KnownSkills.SpecializedAccuracyCheck.Id, out var accuracySkill))
            {
                int numTimesApplied = _skillCountMap[accuracySkill.Id];
                accuracyMod += int.Parse(accuracySkill.OtherAttributes[CheckConstants.ACC_CHECK]) * numTimesApplied;
            }

            int ResolveDamageMod(ICollection<SkillTemplate> attackSkills)
            {   
                int damageMod = LevelDamageModifier;
                foreach(var skill in attackSkills ?? new SkillTemplate[0])
                {                    
                    if (skill.OtherAttributes?.ContainsKey(DamageConstants.DAMAGE_BOOST) != true) continue;
                    damageMod += int.Parse(skill.OtherAttributes[DamageConstants.DAMAGE_BOOST]);
                }
                return damageMod;
            }

            var validAttacks = KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate) ? attacks : attacks.Where(a => !a.IsEquipmentAttack);

            foreach (var attack in validAttacks) 
            {   
                attack.AccuracyMod = accuracyMod;
                attack.DamageMod = 5 + ResolveDamageMod(attack.AttackSkills);
                yield return attack;
            }
        }
              
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks => ResolveAttacks(this._beastTemplate.BasicAttacks).ToArray();

        public int Crisis => _beastTemplate.Crisis;

        public int Defense => ResolveDefense();
        public int DefenseModifier => Defense - _beastTemplate.Defense;
        private ICollection<EquipmentTemplate> Armor => _beastTemplate.Equipment.Where(e => e.Category.IsArmor).ToArray();
        private int ResolveDefense()
        {
            var defense = _beastTemplate.Defense;
            var armor = Armor;
            bool overrides = false;
            if (KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate) && armor.Any())
            {
                var overrideArmor = armor.Where(a => a.StatsModifier?.DefenseOverrides ?? false)
                        .OrderByDescending(a => a.StatsModifier?.DefenseModifier ?? int.MinValue)
                        .FirstOrDefault();
                if(overrideArmor != null)
                {
                    defense = overrideArmor.StatsModifier.DefenseModifier;
                    overrides = true;
                }
                else
                {
                    var bestArmor = armor
                        .OrderByDescending(a => a.StatsModifier?.DefenseModifier ?? int.MinValue)
                        .First();
                    defense += bestArmor.StatsModifier.DefenseModifier;
                }
            }

            if(!overrides)
            {
                var pDefSkillId = KnownSkills.ImprovedDefensesPhysical.Id;
                if (_skillCountMap.TryGetValue(pDefSkillId, out var numPDefTimesApplied))
                {
                    var targetSkill = _skillMap[pDefSkillId];
                    defense += (int.Parse(targetSkill.OtherAttributes[StatsConstants.DEF_BOOST]) * numPDefTimesApplied);
                }

                var mDefSkillId = KnownSkills.ImprovedDefensesMagical.Id;
                if (_skillCountMap.TryGetValue(mDefSkillId, out var numMDefTimesApplied))
                {
                    var targetSkill = _skillMap[mDefSkillId];
                    defense += (int.Parse(targetSkill.OtherAttributes[StatsConstants.DEF_BOOST]) * numMDefTimesApplied);
                }
            }
           
            return defense;
        }

        public string Description 
        {
            get
            {
                return _beastTemplate.Description;
            }
            set
            {
                _beastTemplate.Description = value;
            }
        }
        public Die Dexterity
        {
            get
            {
                return _beastTemplate.Dexterity;
            }
            set
            {
                _beastTemplate.Dexterity = value;
            }
        }

        public IReadOnlyCollection<EquipmentTemplate> Equipment => KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate) ? _beastTemplate.Equipment : Array.Empty<EquipmentTemplate>();

        public int HealthPoints => ResolveHP(_beastTemplate.HealthPoints);

        private int ResolveHP(int healthPoints)
        {
            var result = healthPoints;
            if (_skillCountMap.TryGetValue(KnownSkills.ImprovedHitPoints.Id, out var numTimesApplied))
            {
                var targetSkill = _skillMap[KnownSkills.ImprovedHitPoints.Id];
                var rankHpMultiplier = Model.Rank.GetNumSoldiersReplaced();
                result += (int.Parse(targetSkill.OtherAttributes[StatsConstants.HP_BOOST]) * numTimesApplied * rankHpMultiplier);
            }
            return result;
        }

        public Guid Id
        {
            get
            {
                return _beastTemplate.Id;
            }
            set
            {
                _beastTemplate.Id = value;
            }
        }
        public string ImageFile
        {
            get
            {
                return _beastTemplate.ImageFile;
            }
            set
            {
                _beastTemplate.ImageFile = value;
            }
        }

        public int Initiative => ResolveInitiative(_beastTemplate.Initiative);

        private int ResolveInitiative(int initiative)
        {
            var result = initiative;
            var improvedInitiativeSkillId = KnownSkills.ImprovedInitiative.Id;
            if (_skillCountMap.TryGetValue(improvedInitiativeSkillId, out var numImprovedInitiaveApplied))
            {
                var targetSkill = _skillMap[improvedInitiativeSkillId];
                result += (int.Parse(targetSkill.OtherAttributes[StatsConstants.INIT_BOOST]) * numImprovedInitiaveApplied);
            }           
            return result;
        }

        public Die Insight
        {
            get
            {
                return _beastTemplate.Insight;
            }
            set
            {
                _beastTemplate.Insight = value;
            }
        }
        public int Level
        {
            get
            {
                return _beastTemplate.Level;
            }
            set
            {
                _beastTemplate.Level = value;
            }
        }

        public int LevelAccuracyModifier => _beastTemplate.LevelAccuracyModifier;

        public int LevelDamageModifier => _beastTemplate.LevelDamageModifier;

        public int MagicalDefense => ResolveMagicalDefense();
        public int MagicalDefenseModifier => MagicalDefense - _beastTemplate.MagicalDefense;

        private int ResolveMagicalDefense()
        {
            var magicalDefense = _beastTemplate.MagicalDefense;

            var armor = Armor;
            if (KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate) && armor.Any())
            {
                var bestArmor = armor
                        .OrderByDescending(a => a.StatsModifier?.MagicDefenseModifier ?? int.MinValue)
                        .First();
                magicalDefense += bestArmor.StatsModifier.MagicDefenseModifier;
            }

            var pDefSkillId = KnownSkills.ImprovedDefensesPhysical.Id;
            if (_skillCountMap.TryGetValue(pDefSkillId, out var numPDefTimesApplied))
            {
                var targetSkill = _skillMap[pDefSkillId];
                magicalDefense += (int.Parse(targetSkill.OtherAttributes[StatsConstants.MDEF_BOOST]) * numPDefTimesApplied);
            }

            var mDefSkillId = KnownSkills.ImprovedDefensesMagical.Id;
            if (_skillCountMap.TryGetValue(mDefSkillId, out var numMDefTimesApplied))
            {
                var targetSkill = _skillMap[mDefSkillId];
                magicalDefense += (int.Parse(targetSkill.OtherAttributes[StatsConstants.MDEF_BOOST]) * numMDefTimesApplied);
            }
            return magicalDefense;
        }

        public int MagicPoints => ResolveMP(_beastTemplate.MagicPoints);

        private int ResolveMP(int magicPoints)
        {
            var result = magicPoints;
            var moreMpSkillId = KnownSkills.SpellCasterMoreMP.Id;
            if (_skillCountMap.TryGetValue(moreMpSkillId, out var numMoreMpTimesApplied))
            {
                var targetSkill = _skillMap[moreMpSkillId];
                result += (int.Parse(targetSkill.OtherAttributes[StatsConstants.MP_BOOST]) * numMoreMpTimesApplied);
            }
            var moreSpellsSkillId = KnownSkills.SpellCasterMoreSpells.Id;
            if (_skillCountMap.TryGetValue(moreSpellsSkillId, out var numMoreSpellsTimesApplied))
            {
                var targetSkill = _skillMap[moreSpellsSkillId];
                result += (int.Parse(targetSkill.OtherAttributes[StatsConstants.MP_BOOST]) * numMoreSpellsTimesApplied);
            }
            return result;
        }

        public Die Might
        {
            get
            {
                return _beastTemplate.Might;
            }
            set
            {
                _beastTemplate.Might = value;
            }
        }

        public IBeast Model => _beastTemplate.Model;

        public string Name
        {
            get
            {
                return _beastTemplate.Name;
            }
            set
            {
                _beastTemplate.Name = value;
            }
        }

        public IReadOnlyDictionary<string, BeastResistance> Resistances => ResolveResistances();
           
        private IReadOnlyDictionary<string, BeastResistance> ResolveResistances()
        {
            var damageTypes = DamageConstants.DamageTypeMap.Keys;

            Dictionary<string, BeastResistance> skillAffinities = new Dictionary<string, BeastResistance>();

            bool AffinityTrumps(SkillTemplate skill, BeastResistance beastResistance)
            {
                if (skill.IsVulnerabilitySkill() || skill.IsAbsorptionSkill()) return false;
                var trumps = skill.OtherAttributes?.TryGetValue(DamageConstants.AFFINITY_TRUMPS, out var trumpString) ?? false ? trumpString?.Split(',').Select(i => Guid.Parse(i)).ToHashSet<Guid>() : null;
                return trumps?.Contains(beastResistance.AffinityId) ?? throw new ArgumentException($"Skill '{skill.Name}' has no trumps");
            }

            foreach (var skill in _beastTemplate.Skills.Where(s => s.IsAffinitySkill()))
            {
                var affinity = skill.ToBeastResistance();
                if (skillAffinities.ContainsKey(affinity.DamageType) && !AffinityTrumps(skill, skillAffinities[affinity.DamageType])) continue;
                skillAffinities[affinity.DamageType] = affinity;
            }

            foreach(var damageType in damageTypes.Where(k => !skillAffinities.ContainsKey(k)))
            {
                skillAffinities[damageType] = SkillTemplateExtensions.GetNoAffinitySkill(damageType).ToBeastResistance();
            }
            return skillAffinities;
        }
        
        public IReadOnlyCollection<SkillTemplate> Skills => _beastTemplate.Skills;

        public SpeciesType Species
        {
            get
            {
                return _beastTemplate.Species;
            }
            set
            {
                _beastTemplate.Species = value;
            }
        }

        public IReadOnlyCollection<SpellTemplate> Spells => _beastTemplate.Spells;

        public string Traits
        {
            get
            {
                return _beastTemplate.Traits;
            }
            set
            {
                _beastTemplate.Traits = value;
            }
        }
        public Die WillPower
        {
            get
            {
                return _beastTemplate.WillPower;
            }
            set
            {
                _beastTemplate.WillPower = value;
            }
        }

        public int MagicCheckModifier => ResolveMagicCheckModifier(_beastTemplate.MagicCheckModifier);

        public bool CanBeModified { get; set; } = true;
        public bool CanBeDeleted { get; set; } = true;

        public bool HasDefenseOverride => KnownSkills.UseEquipment.SpeciesCanUse(_beastTemplate) ? Armor.Any(a => a.StatsModifier?.DefenseOverrides ?? false) : false;

        private int ResolveMagicCheckModifier(int magicCheckModifier)
        {
            var checkMod = magicCheckModifier;
            if (_skillMap.TryGetValue(KnownSkills.SpecializedMagicCheck.Id, out var accuracySkill))
            {
                int numTimesApplied = _skillCountMap[accuracySkill.Id];
                checkMod += int.Parse(accuracySkill.OtherAttributes[CheckConstants.ACC_CHECK]) * numTimesApplied;
            }
            return checkMod;
        }

        public Die GetDie(string attributeName) => _beastTemplate.GetDie(attributeName);



        public void UpdateSkills()
        {           
            _skillCountMap = _beastTemplate.Skills.GroupBy(s => s.Id).ToDictionary(g => g.Key, g => g.Count());
            _skillMap = _beastTemplate.Skills.GroupBy(s => s.Id).ToDictionary(g => g.Key, g => g.First());
           
        }

        public string ToText()
        {
            var result = new StringBuilder();
            result.AppendLine($"{Name.ToUpperInvariant()}   Lv {Level} - {Species.Name.ToUpperInvariant()}");
            result.AppendLine();
            result.AppendLine(Description);
            result.AppendLine($"Typical traits: {Traits}");
            result.AppendLine($"DEX {Dexterity} INS {Insight} MIG {Might} WLP {WillPower} | HP {HealthPoints} * {HealthPoints / 2} | MP {MagicPoints} | INIT {Initiative} | DEF {Defense} | M DEF {MagicalDefense}");
            result.AppendLine("RESISTANCES");
            result.AppendLine(string.Join("|", Resistances.Where(r => !string.IsNullOrWhiteSpace(r.Value.Affinity)).Select(r => $"{r.Key} {r.Value.Affinity}")));
            result.AppendLine("BASIC ATTACKS");
            foreach(var attack in this.AllAttacks)
            {
                var attackType = attack.IsRanged ? "RANGED" : "MELEE";
                var accuracyMod = attack.AccuracyMod != 0 ? $" + {attack.AccuracyMod}" : string.Empty;
                result.AppendLine($"{attackType} {attack.Name} * [{attack.Attribute1} + {attack.Attribute2}{accuracyMod}] * [HR + {attack.DamageMod}] {attack.DamageType.Name.ToLowerInvariant()}");
                foreach(var skill in attack.AttackSkills)
                {
                    result.AppendLine(skill.Text);
                }
                result.AppendLine();
            }
            if(this.Spells.Any())
            {
                result.AppendLine("SPELLS");
                foreach (var spell in this.Spells)
                {
                    var isOff = spell.IsOffensive ? " OFF " : string.Empty;
                    var accMod = MagicCheckModifier != 0 ? $" + {MagicCheckModifier}" : string.Empty;
                    result.AppendLine($"{spell.Name}{isOff} * [{spell.Attribute1} {spell.Attribute2}{accMod}] * {spell.MagicPointCost} MP * {spell.Target} * {spell.Duration}");
                    result.AppendLine(spell.Description);
                }
            }
           
            if(this.Actions.Any())
            {
                result.AppendLine("OTHER ACTIONS");
                foreach (var action in this.Actions)
                {
                    result.AppendLine($"{action.Name} * {action.Effect}");
                }
            }

            if (this.Skills.Any(s => s.IsSpecialRule))
            {
                result.AppendLine("SPECIAL RULES");
                foreach(var skill in this.Skills.Where(s => s.IsSpecialRule))
                {
                    result.AppendLine($"{skill.Name} * {skill.Text}");
                }
            }

            return result.ToString();
        }
    }
}
