using FabulaUltimaDatabase;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary.Models;

namespace FabulaUltimaSkillLibrary
{
    public class Resolver
    {
        private readonly Instance _instance;
        private readonly SpecialAttackIndex _specialAttackIndex;

        public Resolver(Instance instance, SpecialAttackIndex specialAttackIndex) 
        { 
            _instance = instance;
            _specialAttackIndex = specialAttackIndex;
        }

        public SkillOutputData ResolveSkills(IBeastTemplate npc, SkillInputData inputData)
        {
            var speciesSkillSlots = GetSkillSlotsFromSpecies(npc.Species);
            var levelSkillSlots = GetSkillsSlotsFromLevel(npc.Level);
            var vulnerabilitySkillSlots = GetSkillsSlotsFromVulnerabilities(npc);

            var resolvedSkills = ResolveSkillsInternal(npc, inputData).ToArray();

            var grantedSkillSlots = resolvedSkills.Where(s => s == null);

            var totalSkillSlots = speciesSkillSlots
                                    .Concat(levelSkillSlots)
                                    .Concat(vulnerabilitySkillSlots)
                                    .Concat(grantedSkillSlots)
                                    .ToArray();

            
            var skillQueue = new Queue<(SkillTemplate skill, Guid? targetId)?>(resolvedSkills.Where(s => s != null));

            for(int index = 0; index < totalSkillSlots.Count(); index++)
            {   
                if (totalSkillSlots[index] != null) continue;
                if (!skillQueue.Any()) break;
                var skill = skillQueue.Dequeue();
                totalSkillSlots[index] = skill;
            }

            return new SkillOutputData
            {
                SkillSlots = totalSkillSlots
            };
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveSkillsInternal(
            IBeastTemplate npc,
            SkillInputData inputData)
        {
            var statsSkills = ResolveStatsSkills(npc, inputData);
            var specialAttacks = ResolveSpecialAttacks(npc, inputData.AttackModifiers, npc.AllAttacks);
            var equipmentSkills = ResolveEquipmentSkills(npc);
            var spellSkills = ResolveSpellSkills(npc, inputData.MaxMP);
            var resistanceSkills = ResolveResistances(npc.Species, npc.Resistances.Values);
            var vulnerbilitySkills = ResolveVulnerbilities(npc.Resistances.Values.Where(r => r.AffinityId == DamageConstants.VULNERABLE));
            var immunitySkills = ResolveImmunities(npc.Species, npc.Resistances.Values.Where(r => r.AffinityId == DamageConstants.IMMUNE));
            var absorptionSkills = ResolveAbsorption(npc.Species, npc.Resistances.Values.Where(r => r.AffinityId == DamageConstants.ABSORBS));
            var checkSkills = ResolveChecks(npc, inputData);
            var speciesSkills = ResolveSpecies(npc);

            return statsSkills
                .Concat(specialAttacks)                
                .Concat(spellSkills)
                .Concat(resistanceSkills)
                .Concat(checkSkills)
                .Concat(vulnerbilitySkills)
                .Concat(immunitySkills)
                .Concat(absorptionSkills)
                .Concat(equipmentSkills)
                .Concat(speciesSkills); 
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveAbsorption(SpeciesType species, IEnumerable<BeastResistance> absorbs)
        {
            var skills = absorbs.Select(r => KnownSkills.GetAbsorptionSkill(r.DamageTypeId));                                   

            foreach (var skill in skills)
            {
                yield return (skill, null);
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveImmunities(SpeciesType species, IEnumerable<BeastResistance> immunities)
        {
            var skills = immunities.Select(r => KnownSkills.GetImmunitySkill(r.DamageTypeId))
                                   .Where(s =>
                                   {   
                                       return s.OtherAttributes?.FreeSpecies?.Contains(species.Id) != true;
                                   }); // exclude immunities that will be granted for species

            

            foreach (var skill in skills)
            {
                yield return (skill, null);
            }

            int freeImmunities = _instance.GetNumFreeImmunities(species);
            foreach(var _ in Enumerable.Range(0, freeImmunities))
            {
                yield return null;
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveSpecies(IBeastTemplate npc)
        {
            foreach(var freeSkill in KnownSkills.GetAllKnownSkills()
                                .Where(s => 
                                { 
                                    return s.OtherAttributes?.FreeSpecies?.Contains(npc.Species.Id) == true;
                                }))
            {
                yield return (freeSkill, null);
                yield return null;
            }            
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveChecks(IBeastTemplate npc, SkillInputData inputData)
        {
            var basicAttack = npc.AllAttacks?.FirstOrDefault();
            if (basicAttack == null) yield break;
            var firstAttackMod = basicAttack.AttackMod;
            var levelAttackMod = npc.LevelAccuracyModifier;
            var totalAttackMod = firstAttackMod + levelAttackMod;
            var givenAttackMod = inputData.AttackModifiers[basicAttack.Id].AtkMod;
            var modDiff = givenAttackMod - totalAttackMod;

            var accuracySpecializedSkill = KnownSkills.SpecializedAccuracyCheck;
            if(modDiff == int.Parse(accuracySpecializedSkill.OtherAttributes[CheckConstants.ACC_CHECK]))
            {
                yield return (accuracySpecializedSkill, null);
            }

        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveVulnerbilities(IEnumerable<BeastResistance> vulnerbilities)
        {
            foreach (var vul in vulnerbilities)
            {
                yield return (KnownSkills.GetVulnerabilitySkill(vul.DamageTypeId), null);                
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveResistances(SpeciesType species, IEnumerable<BeastResistance> resistances)
        {
            
            var resistanceSkills = resistances.Where(r => r.AffinityId == DamageConstants.RESISTANT)
                                    .Select(r => KnownSkills.GetResistanceSkill(r.DamageTypeId))
                                    .Where(s =>
                                    {
                                        if(s.OtherAttributes.TryGetValue(SpeciesConstants.FREE_SPECIES, out var value))
                                        {
                                            var speciesTypeId = Guid.Parse(value);
                                            return species.Id != speciesTypeId;
                                        }
                                        return true;
                                    }); // exclude resistances that will be granted for species

            int freeResistances = _instance.GetNumFreeResistances(species);
            var impliedResistanceFromAbsorptionSkills = resistances
                                   .Where(r => r.AffinityId == DamageConstants.ABSORBS)
                                   .Select(r => KnownSkills.GetAbsorptionSkill(r.DamageTypeId))
                                   .Select(a =>
                                   {
                                       if (freeResistances > 0)
                                       {
                                           return a.OtherAttributes?
                                                    .OtherKnownSkillsRequired?
                                                    .Select(id => KnownSkills.GetKnownSkill(id))
                                                    .Single(s => s.OtherAttributes[DamageConstants.AFFINITY_ID] == DamageConstants.RESISTANT.ToString());
                                       }   
                                       return null;
                                   }).
                                   Where(s => s != null); // exclude absorptions that will be granted for species

            var skills = resistanceSkills.Concat(impliedResistanceFromAbsorptionSkills).ToList();

            foreach (var skill in skills)
            {
                yield return (skill, null);
            }

            
            var numFreeSlots = (skills.Count() + freeResistances) / 2;
            foreach(var _ in Enumerable.Range(0, numFreeSlots))
            {
                yield return null;
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveSpellSkills(IBeastTemplate npc, int maxMP)
        {
            if((npc.Spells?.Any() ?? false) == false) 
            { 
                yield break; 
            }
            var moreMPSkill = KnownSkills.SpellCasterMoreMP;            
            var spellCount = npc.Spells.Count();
            var calcMp = npc.MagicPoints;
            var mpDiff = maxMP - calcMp;
            var numBoostedSpellSkills = mpDiff / int.Parse(moreMPSkill.OtherAttributes[StatsConstants.MP_BOOST]);
            var boostedMpSkills = Enumerable.Range(0, numBoostedSpellSkills).Select(_ => moreMPSkill);

            var moreSpellsSkill = KnownSkills.SpellCasterMoreSpells;
            var remainingSpellSlots = (spellCount - numBoostedSpellSkills) / int.Parse(moreSpellsSkill.OtherAttributes[StatsConstants.NUM_SPELLS]);
            var boostedSpellsSkills = Enumerable.Range(0, remainingSpellSlots).Select(_ => moreSpellsSkill);

            foreach(var skill in boostedMpSkills
                .Concat(boostedSpellsSkills))
            {
                yield return (skill, null);
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveEquipmentSkills(IBeastTemplate npc)
        {
            var skill = KnownSkills.UseEquipment;            
            if (npc.Equipment?.Any() == true && npc.Species.Id != SpeciesConstants.HUMANOID) yield return (skill, null);            
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveSpecialAttacks(
            IBeastTemplate npc,
            IDictionary<Guid, AttackModifier> attackModifiers, 
            IEnumerable<BasicAttackTemplate> basicAttacks)
        {
            var attackDictionary = basicAttacks?.ToDictionary(a => a.Id, a => a) ?? new Dictionary<Guid, BasicAttackTemplate>();
            foreach((Guid attackId, string text) in attackModifiers.Where(m => !string.IsNullOrWhiteSpace(m.Value.Text)).Select(m => (m.Key, m.Value.Text)))
            {
                
                var specialAttackSkills = _specialAttackIndex.GetSpecialAttacks(text);
                var attack = attackDictionary[attackId];
                foreach(var specialAttackSkill in specialAttackSkills)
                {
                    yield return (specialAttackSkill, attack.Id);
                    if (specialAttackSkill.OtherAttributes.TryGetValue(KnownSkills.IS_SPECIAL_ATTACK_DETRIMENT, out var isFreeStr) &&
                    bool.Parse(isFreeStr))
                    {
                        yield return null;
                        yield return null; // not just free, but gives a skill slot back
                    }
                }
            }

            var improvedDamageSkill = KnownSkills.ImprovedDamageAttack;

            foreach(var attack in npc.AllAttacks)
            {
                var damageMod = attack.DamageMod;
                var levelMod = npc.LevelDamageModifier;
                var totalCalcMod = damageMod + levelMod;
                var givenDamageMod = attackModifiers[attack.Id].DamMod;

                var damModDiff = givenDamageMod - totalCalcMod;
                if(damModDiff == int.Parse(improvedDamageSkill.OtherAttributes[DamageConstants.DAMAGE_BOOST]))
                {
                    yield return (improvedDamageSkill, attack.Id);
                }
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> ResolveStatsSkills(IBeastTemplate npc, SkillInputData inputData)
        {
            var improvedHitPoints = KnownSkills.ImprovedHitPoints;
            var hpDiff = inputData.MaxHP - npc.HealthPoints;
            var numImprovedHPSkills = hpDiff / int.Parse(improvedHitPoints.OtherAttributes[StatsConstants.HP_BOOST]);
            var aggregatedSkills = Enumerable.Range(0, numImprovedHPSkills).Select(_ => improvedHitPoints);

            var improvedInitiative = KnownSkills.ImprovedInitiative;
            if(inputData.Init == npc.Initiative + int.Parse(improvedInitiative.OtherAttributes[StatsConstants.INIT_BOOST]))
            {
                aggregatedSkills = aggregatedSkills.Concat(new [] { improvedInitiative });
            }


            var morePDef = KnownSkills.ImprovedDefensesPhysical;
            var moreMDef = KnownSkills.ImprovedDefensesMagical;
            var pdefMinMod = int.Parse(moreMDef.OtherAttributes[StatsConstants.DEF_BOOST]);

            var numPDef = 0;
            var numMDef = 0;

            var mDefModMinusArmor = inputData.MDefMod - (npc.Equipment?.Sum(e => e.StatsModifier?.MagicDefenseModifier ?? 0) ?? 0);
            var pDefModMinusArmor = inputData.DefMod - (npc.Equipment?.Sum(e => e.StatsModifier?.DefenseModifier ?? 0) ?? 0);
            switch (mDefModMinusArmor)
            {
                case 0:
                    numPDef = 0;
                    numMDef = 0;
                    break;
                case 1:
                    numPDef = 1;
                    numMDef = 0;
                    break;
                case 2:
                    if(inputData.DefOverride == null && pDefModMinusArmor == pdefMinMod)
                    {
                        numPDef = 0;
                        numMDef = 1;
                    }
                    else
                    {
                        numPDef = 2;
                        numMDef = 0;
                    }                    
                    break;
                case 3:
                    numPDef = 1;
                    numMDef = 1;
                    break;
                case 4:
                    numPDef = 0;
                    numMDef = 2;
                    break;
                default:
                    throw new InvalidDefenseBoostException(inputData.MDefMod, inputData.DefMod, inputData.DefOverride);
            }

            aggregatedSkills = aggregatedSkills
                                .Concat(Enumerable.Range(0, numPDef).Select(_ => morePDef))
                                .Concat(Enumerable.Range(0, numMDef).Select(_ => moreMDef));


            foreach (var skill in aggregatedSkills)
            {
                yield return (skill, null);
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> GetSkillsSlotsFromVulnerabilities(IBeastTemplate npc)
        {
            var resistances = npc.Resistances;
            var vulChoices = _instance.GetBuiltInVulnerbilityChoices(npc.Species);
            var builtInVulnerabilities = vulChoices.VulnerabilityChoices.Select(r => r.DamageTypeId).ToHashSet();
            var choicesLeft = vulChoices.NumVulnerabilityChoices;
            foreach(var resistance in resistances.Values.Where(r => r.AffinityId == DamageConstants.VULNERABLE))
            {
                yield return null; // for the upcoming vulnerability
                if (builtInVulnerabilities.Contains(resistance.DamageTypeId) && choicesLeft > 0)
                {
                    choicesLeft--;
                    continue;
                }

                var numSkillsGranted = _instance.GetSkillBonus(resistance.DamageTypeId);                
                foreach (var _ in Enumerable.Range(0, numSkillsGranted))
                {                    
                    yield return null; // extra slot
                }
            }
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> GetSkillsSlotsFromLevel(int level)
        {
            var numSkillSlots = level / 10;
            return Enumerable.Range(0, numSkillSlots).Select(_ => ((SkillTemplate skill, Guid? targetId)?)null);
        }

        private IEnumerable<(SkillTemplate skill, Guid? targetId)?> GetSkillSlotsFromSpecies(SpeciesType species)
        {
            var numSkillSlots = _instance.GetNumSkills(species);
            return Enumerable.Range(0, numSkillSlots).Select(_ => ((SkillTemplate skill, Guid? targetId) ?) null);
        }

        public void RebuildSpecialAttackIndex()
        {
            _specialAttackIndex.Rebuild();
        }
    }
}