using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibrary
{
    public static class SkillTemplateExtensions
    {
        public static bool IsVulnerabilitySkill(this SkillTemplate skillTemplate)
        {
            if(skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(DamageConstants.AFFINITY_ID, out var value) ? value == DamageConstants.VULNERABLE.ToString() : false;
        }

        public static bool IsImmunitySkill(this SkillTemplate skillTemplate)
        {            
            return skillTemplate.IsAffinitySkill(DamageConstants.IMMUNE);
        }

        public static bool IsResistanceSkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.IsAffinitySkill(DamageConstants.RESISTANT);
        }

        public static bool IsAbsorptionSkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.IsAffinitySkill(DamageConstants.ABSORBS);
        }

        public static bool IsAffinitySkill(this SkillTemplate skillTemplate, Guid affinityId)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(DamageConstants.AFFINITY_ID, out var value) ? value == affinityId.ToString() : false;
        }

        public static bool IsSpeciesSkill(this SkillTemplate skillTemplate) 
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(SpeciesConstants.IS_SPECIES_SKILL, out var value) ? value == true.ToString() : false;
        }

        public static bool SpeciesCanUse(this SkillTemplate skillTemplate, IBeastTemplate template)
        {
            if (template.Species == null) return false;
            if (skillTemplate.OtherAttributes == null) return true;
            return skillTemplate.OtherAttributes.TryGetValue(SpeciesConstants.BLOCKED_SPECIES, out var value) ? value != template.Species.Id.ToString() : true;
        }

        public static bool IsAffinitySkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.ContainsKey(DamageConstants.AFFINITY_ID);
        }

        public static bool IsSpellcasterSkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.ContainsKey(StatsConstants.NUM_SPELLS);
        }

        public const string RESOLVED = "Resolved";
        public static bool IsResolved(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(RESOLVED, out var valStr) ? bool.Parse(valStr) : false;
        }

        public static SkillTemplate SetResolved(this SkillTemplate template, bool resolved)
        {
            template.OtherAttributes = template.OtherAttributes ?? new SkillAttributeCollection();
            template.OtherAttributes[RESOLVED] = resolved.ToString();
            return template;
        }

        public static bool ModifiesAttack(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            if (skillTemplate.OtherAttributes.IsSpecialAttack == true) return true;
            return skillTemplate.OtherAttributes.ContainsKey(DamageConstants.DAMAGE_BOOST);
        }

        public static bool IsFreeSkillForSpecies(this SkillTemplate skillTemplate, SpeciesType species)
        {            
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(SpeciesConstants.FREE_SPECIES, out var value) ? value.Contains(species.Id.ToString()) : false;
        }

        public static BeastResistance ToBeastResistance(this SkillTemplate skillTemplate)
        {
            if (!IsAffinitySkill(skillTemplate)) throw new ArgumentException("not a resistance skill");
            var affinityId = Guid.Parse(skillTemplate.OtherAttributes[DamageConstants.AFFINITY_ID] ?? throw new Exception("unset"));
            var damageType = skillTemplate.OtherAttributes[DamageConstants.DAMAGE_TYPE_NAME];
            var resolved = skillTemplate.IsResolved();
            return new BeastResistance
            {
                AffinityId = affinityId,               
                Affinity = DamageConstants.AffinityMap[affinityId],
                DamageType = damageType,
                DamageTypeId = DamageConstants.DamageTypeMap[damageType.ToLowerInvariant()],
                SkillId = skillTemplate.Id,
                Resolved = resolved,
            };
        }

        public static SkillTemplate GetNoAffinitySkill(string damageType)
        {
            return
                new SkillTemplate(Guid.Parse("0599471f-6102-428b-9577-f72835db5e0d"))
                {
                    Name = $"No {damageType} Affinity",
                    TargetType = typeof(BeastResistance),
                    IsSpecialRule = false,
                    Keywords = new HashSet<string> { "no", "damageType", "affinity" },
                    OtherAttributes = new SkillAttributeCollection
                    {
                        [DamageConstants.AFFINITY_ID] = DamageConstants.NO_AFFINITY.ToString(),
                        [DamageConstants.AFFINITY_TRUMPS] = string.Join(',', new string[] { DamageConstants.VULNERABLE.ToString() }),
                        [DamageConstants.DAMAGE_TYPE_NAME] = damageType,
                    }
                };
        }

        public static SkillTemplate Clone(this SkillTemplate skillTemplate)
        {
            return new SkillTemplate(skillTemplate.Id)
            {
                Name = skillTemplate.Name,
                TargetType = skillTemplate.TargetType,
                IsSpecialRule = skillTemplate.IsSpecialRule,
                Keywords = skillTemplate.Keywords,
                Text = skillTemplate.Text,
                OtherAttributes = skillTemplate.OtherAttributes?.Clone()
            };
        }
    }
}
