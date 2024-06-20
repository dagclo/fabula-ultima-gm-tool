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

        public static bool IsSpeciesSkill(this SkillTemplate skillTemplate) 
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.TryGetValue(SpeciesConstants.IS_SPECIES_SKILL, out var value) ? value == true.ToString() : false;
        }

        public static bool SpeciesCanUse(this SkillTemplate skillTemplate, IBeastTemplate template)
        {
            if (skillTemplate.OtherAttributes == null) return true;
            return skillTemplate.OtherAttributes.TryGetValue(SpeciesConstants.BLOCKED_SPECIES, out var value) ? value != template.Species.Id.ToString() : true;
        }

        public static bool IsResistanceSkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.ContainsKey(DamageConstants.AFFINITY_ID);
        }

        public static bool IsSpellcasterSkill(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            return skillTemplate.OtherAttributes.ContainsKey(StatsConstants.NUM_SPELLS);
        }

        public static bool ModifiesAttack(this SkillTemplate skillTemplate)
        {
            if (skillTemplate.OtherAttributes == null) return false;
            if (skillTemplate.OtherAttributes.IsSpecialAttack == true) return true;
            return skillTemplate.OtherAttributes.ContainsKey(DamageConstants.DAMAGE_BOOST);
        }

        public static BeastResistance ToBeastResistance(this SkillTemplate skillTemplate)
        {
            if (!IsResistanceSkill(skillTemplate)) throw new ArgumentException("not a resistance skill");
            var affinityId = Guid.Parse(skillTemplate.OtherAttributes[DamageConstants.AFFINITY_ID]);
            var damageType = skillTemplate.OtherAttributes[DamageConstants.DAMAGE_TYPE_NAME];
            return new BeastResistance
            {
                AffinityId = affinityId,               
                Affinity = DamageConstants.AffinityMap[affinityId],
                DamageType = damageType,
                DamageTypeId = DamageConstants.DamageTypeMap[damageType.ToLowerInvariant()]
            };
        }
    }
}
