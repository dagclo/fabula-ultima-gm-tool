namespace FabulaUltimaNpc
{
    public class SpellTemplate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Attribute1 { get; set; }
        public string? Attribute2 { get; set; }
        public bool IsOffensive { get; set; }
        public string? Duration { get; set; }
        public string? Target { get; set; }
        public int MagicPointCost { get; set; }
        public string? Description { get; set; }
        public int? DamageModifier { get; set; }
        public DamageType? DamageType { get; set; }

        public SpellTemplate Clone()
        {
            return new SpellTemplate
            {
                Id = Id,
                Name = Name,
                Attribute1 = Attribute1,
                Attribute2 = Attribute2,
                IsOffensive = IsOffensive,
                Duration = Duration,
                Target = Target,
                MagicPointCost = MagicPointCost,
                Description = Description,
                DamageModifier = DamageModifier,
                DamageType = DamageType,
            };
        }
    }
}
