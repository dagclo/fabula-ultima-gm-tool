﻿namespace FabulaUltimaNpc
{
    public class BasicAttackTemplate
    {
        public DamageType DamageType { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public int AttackMod { get; set; }
        public int DamageMod { get; set; }
        public bool IsRanged { get; set; }
        public ICollection<SkillTemplate> AttackSkills { get; set; }

        public BasicAttackTemplate Clone()
        {
            return new BasicAttackTemplate
            {
                DamageType = DamageType,
                Id = Id,
                Name = Name,
                Attribute1 = Attribute1,
                Attribute2 = Attribute2,
                AttackMod = AttackMod,
                DamageMod = DamageMod,
                IsRanged = IsRanged,
                AttackSkills = AttackSkills,
            };
        }
    }
}
