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

        private IDictionary<string, BeastResistance> _resistances = new Dictionary<string, BeastResistance>();
        public IReadOnlyDictionary<string, BeastResistance> Resistances
        {
            get => _resistances as IReadOnlyDictionary<string, BeastResistance>;
            set => _resistances = value as IDictionary<string, BeastResistance>;
        }

        private ICollection<BasicAttackTemplate> _basicAttacks = new List<BasicAttackTemplate>();
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks
        {
            get => _basicAttacks as IReadOnlyCollection<BasicAttackTemplate>;
            set => _basicAttacks = value as ICollection<BasicAttackTemplate>;
        }

        public void AddBasicAttack(BasicAttackTemplate basicAttack) => _basicAttacks.Add(basicAttack);
        public void RemoveBasicAttack(BasicAttackTemplate basicAttack) => _basicAttacks.Remove(basicAttack);

        private ICollection<SpellTemplate> _spells = new List<SpellTemplate>();
        public IReadOnlyCollection<SpellTemplate> Spells
        {
            get => _spells as IReadOnlyCollection<SpellTemplate>;
            set => _spells = value as ICollection<SpellTemplate>;
        }
        public void AddSpell(SpellTemplate spell) => _spells.Add(spell);
        public void RemoveSpell(SpellTemplate spell) => _spells.Remove(spell);

        private ICollection<EquipmentTemplate> _equipment = new List<EquipmentTemplate>();
        public IReadOnlyCollection<EquipmentTemplate> Equipment
        {
            get => _equipment as IReadOnlyCollection<EquipmentTemplate>;
            set => _equipment = value as ICollection<EquipmentTemplate>;
        }

        public void AddEquipment(EquipmentTemplate equipment) => _equipment.Add(equipment);
        public void RemoveEquipment(EquipmentTemplate equipment) => _equipment.Remove(equipment);
        public bool HasEquipment(EquipmentTemplate equipment) => _equipment.Contains(equipment);

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
        
        private ICollection<ActionTemplate> _actions = new List<ActionTemplate>();
        public IReadOnlyCollection<ActionTemplate> Actions
        {
            get => _actions as IReadOnlyCollection<ActionTemplate>;
            set => _actions = value as ICollection<ActionTemplate>;
        }

        public void AddAction(ActionTemplate action) => _actions.Add(action);
        public void RemoveAction(ActionTemplate action) => _actions.Remove(action);

        public Rank Rank { get; set; } = Rank.Soldier;

        public void RemoveSkill(SkillTemplate skill) => _skillTemplates.Remove(skill);
        public void AddSkill(SkillTemplate skill) => _skillTemplates.Add(skill);

        public void RemoveSkill(Guid skillId)
        {
            var skill = _skillTemplates.FirstOrDefault(s => s.Id == skillId);
            if(skill != null) _skillTemplates.Remove(skill);
        }
    }
}
