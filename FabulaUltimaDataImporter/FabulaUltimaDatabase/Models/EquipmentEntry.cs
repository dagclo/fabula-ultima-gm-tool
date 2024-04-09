namespace FabulaUltimaDatabase.Models
{
    public class EquipmentEntry
    {
        public string Name { get; set; }
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Attribute1 { get; set; }
        public string? Attribute2 { get; set; }        
        public bool? IsMartial { get; set; }
        public int Cost { get; set; }
        public int? DamageMod { get; set; }
        public int? AttackMod { get; set; }
        public Guid? DamageType { get; set; }
        public int? NumHands { get; set; }
        public string Quality { get; set; }
        public int? DefenseModification { get; set; }
        public int? DefenseOverride { get; set; }
        public int? MagicDefenceModification { get; set; }
        public int? InitiativeModification { get; set; }
    }
}
