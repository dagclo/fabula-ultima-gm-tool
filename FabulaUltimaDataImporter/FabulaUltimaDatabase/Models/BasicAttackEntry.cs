namespace FabulaUltimaDatabase.Models
{
    public class BasicAttackEntry
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Attribute2 { get; set; }
        public string? Attribute1 { get; set; }
        public int? DamageMod { get; set; }
        public Guid? DamageType { get; set; }
        public bool? IsRanged { get; set; }
        public int AttackMod { get; set; }
    }
}
