namespace FabulaUltimaNpc
{
    public class BasicAttackTemplate
    {
        public DamageType DamageType { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public int AccuracyMod { get; set; }
        public int DamageMod { get; set; }
        public bool IsRanged { get; set; }
        public ICollection<SkillTemplate> AttackSkills { get; set; }

        public bool IsEquipmentAttack { init; get; } = false;

        public BasicAttackTemplate Clone()
        {
            return new BasicAttackTemplate
            {
                DamageType = DamageType,
                Id = Id,
                Name = Name,
                Attribute1 = Attribute1,
                Attribute2 = Attribute2,
                AccuracyMod = AccuracyMod,
                DamageMod = DamageMod,
                IsRanged = IsRanged,
                AttackSkills = AttackSkills?.Select(s => s.Clone()).ToList() ?? new List<SkillTemplate>(),
                IsEquipmentAttack = IsEquipmentAttack,
            };
        }
    }
}
