namespace FabulaUltimaNpc
{
    public interface IBeast
    {
        // persisted
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public string Traits { get; set; }
        public SpeciesType Species { get; set; }
        public Die Insight { get; set; }
        public Die Dexterity { get; set; }
        public Die Might { get; set; }
        public Die WillPower { get; set; }
        public string ImageFile { get; set; }

        public Rank Rank { get; }

        public IReadOnlyDictionary<string, BeastResistance> Resistances { get; }
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks { get; }
        public IReadOnlyCollection<SpellTemplate> Spells { get; }
        public IReadOnlyCollection<EquipmentTemplate> Equipment { get; }
        public IReadOnlyCollection<SkillTemplate> Skills { get; }
        public IReadOnlyCollection<ActionTemplate> Actions { get; }
    }
}
