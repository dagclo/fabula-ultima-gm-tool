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

        void AddEquipment(EquipmentTemplate equipment);
        void RemoveEquipment(EquipmentTemplate equipment);
        bool HasEquipment(EquipmentTemplate equipment);
        void AddSkill(SkillTemplate skill);
        void RemoveSkill(SkillTemplate skill);

        void AddBasicAttack(BasicAttackTemplate basicAttack);
        void RemoveBasicAttack(BasicAttackTemplate basicAttack);
        void AddAction(ActionTemplate action);
        void RemoveAction(ActionTemplate action);
        void AddSpell(SpellTemplate spell);
        void RemoveSpell(SpellTemplate spell);
    }
}
