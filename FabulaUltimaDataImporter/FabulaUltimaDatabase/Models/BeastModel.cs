using FabulaUltimaNpc;

namespace FabulaUltimaDatabase.Models
{
    public class BeastModel : IBeast
    {
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

        public IReadOnlyDictionary<string, BeastResistance> Resistances { get; set; } = new Dictionary<string, BeastResistance>();

        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks { get; set; } = new List<BasicAttackTemplate>();

        public IReadOnlyCollection<SpellTemplate> Spells { get; set; } = new List<SpellTemplate>();

        public IReadOnlyCollection<EquipmentTemplate> Equipment { get; set; } = new List<EquipmentTemplate>();

        public IReadOnlyCollection<SkillTemplate> Skills { get; set; } = new List<SkillTemplate>();
        public IReadOnlyCollection<ActionTemplate> Actions { get; set; } = new List<ActionTemplate>();
        public Rank Rank => Rank.Soldier;
    }
}
