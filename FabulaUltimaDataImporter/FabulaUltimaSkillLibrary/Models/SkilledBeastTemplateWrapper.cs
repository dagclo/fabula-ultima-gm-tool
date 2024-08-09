using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibrary.Models
{
    public class SkilledBeastTemplateWrapper : IBeastTemplate
    {
        private readonly IBeastTemplate _beastTemplate;
        private readonly IReadOnlyDictionary<Guid, int> _skillCountMap;
        private readonly IReadOnlyDictionary<Guid, SkillTemplate> _skillMap;        

        public SkilledBeastTemplateWrapper(IBeastTemplate beastTemplate)
        {
            _beastTemplate = beastTemplate;
            _skillCountMap = beastTemplate.Skills.GroupBy(s => s.Id).ToDictionary(g => g.Key, g => g.Count());
            _skillMap = beastTemplate.Skills.GroupBy(s => s.Id).ToDictionary(g => g.Key, g => g.First());
        }

        public IReadOnlyCollection<ActionTemplate> Actions => _beastTemplate.Actions;

        public IEnumerable<BasicAttackTemplate> AllAttacks => ResolveAttacks(this._beastTemplate.AllAttacks).ToArray();

        public IEnumerable<BasicAttackTemplate> ResolveAttacks(IEnumerable<BasicAttackTemplate> attacks)
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

            foreach (var attack in attacks) 
            {
                var updatedAttack = attack.Clone(); // want to preserve original value
                updatedAttack.AccuracyMod += accuracyMod;
                updatedAttack.DamageMod += ResolveDamageMod(attack.AttackSkills);
                yield return updatedAttack;
            }
        }
              
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks => ResolveAttacks(this._beastTemplate.BasicAttacks).ToArray();

        public int Crisis => _beastTemplate.Crisis;

        public int Defense => ResolveDefense();
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
                result += (int.Parse(targetSkill.OtherAttributes[StatsConstants.HP_BOOST]) * numTimesApplied);
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
                var trumps = skill.OtherAttributes?[DamageConstants.AFFINITY_TRUMPS].Split(',').Select(i => Guid.Parse(i)).ToHashSet<Guid>();
                return trumps?.Contains(beastResistance.AffinityId) ?? throw new ArgumentException("Skill has no trumps");
            }

            foreach (var skill in _beastTemplate.Skills.Where(s => s.IsResistanceSkill()))
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

        public bool Immutable { get; set; }

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

        public void AddSkill(SkillTemplate skill)
        {
            if (this.Skills is not ICollection<SkillTemplate> skillCollection) return;
            if (skillCollection.Any(s => s.Id == skill.Id)) return;
            skillCollection.Add(skill);
        }

        public void RemoveAffinitySkill(Guid skillId)
        {
            if (this.Skills is not ICollection<SkillTemplate> skillCollection) return;
            if (!skillCollection.Any(s => s.Id == skillId)) return;
            skillCollection.Remove(skillCollection.Single(s => s.Id == skillId));
        }
    }
}
