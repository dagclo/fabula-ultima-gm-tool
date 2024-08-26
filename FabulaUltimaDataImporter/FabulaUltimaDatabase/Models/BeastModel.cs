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

        public IDictionary<string, BeastResistance> Resistances { get; set; } = new Dictionary<string, BeastResistance>();

        public ICollection<BasicAttackTemplate> BasicAttacks { get; set; } = new List<BasicAttackTemplate>();

        public ICollection<SpellTemplate> Spells { get; set; } = new List<SpellTemplate>();

        public ICollection<EquipmentTemplate> Equipment { get; set; } = new List<EquipmentTemplate>();

        private ICollection<SkillTemplate> _skillTemplates = new List<SkillTemplate>();
        public IReadOnlyCollection<SkillTemplate> Skills
        {
            get
            {
                return _skillTemplates as IReadOnlyCollection<SkillTemplate>;
            }
            set
            {
                _skillTemplates = value as ICollection<SkillTemplate>;
            }
        }
        public ICollection<ActionTemplate> Actions { get; set; } = new List<ActionTemplate>();
        public Rank Rank { get; set; } = Rank.Soldier;

        public void RemoveSkill(SkillTemplate skill) => _skillTemplates.Remove(skill);
        public void AddSkill(SkillTemplate skill) => _skillTemplates.Add(skill);
    }
}
