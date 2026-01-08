namespace FabulaUltimaDatabase.Models
{
    public class SpellEntry
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Duration { get; set; }
        public string? Target { get; set; }
        public int MagicPointCost { get; set; }
        public string? Description { get; set; }
        public string? Attribute1 { get; set; }
        public string? Attribute2 { get; set; }
        public bool? IsOffensive { get; set; }
        public Guid? DamageType { get; set; }
        public int? DamageModifier { get; set; }
    }
}
