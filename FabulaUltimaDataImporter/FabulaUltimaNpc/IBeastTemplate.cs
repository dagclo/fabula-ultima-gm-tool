namespace FabulaUltimaNpc
{
    public interface IBeastTemplate
    {
        IReadOnlyCollection<ActionTemplate> Actions { get; }
        IEnumerable<BasicAttackTemplate> AllAttacks { get; }
        IReadOnlyCollection<BasicAttackTemplate> BasicAttacks { get; }
        int Crisis { get; }
        int Defense { get; }
        string Description { get; set; }
        Die Dexterity { get; set; }
        IReadOnlyCollection<EquipmentTemplate> Equipment { get; }
        int HealthPoints { get; }
        Guid Id { get; set; }
        string ImageFile { get; set; }
        int Initiative { get; }
        Die Insight { get; set; }
        int Level { get; set; }
        int LevelAccuracyModifier { get; }
        int LevelDamageModifier { get; }
        int MagicCheckModifier { get; }
        int MagicalDefense { get; }
        int MagicPoints { get; }
        Die Might { get; set; }
        IBeast Model { get; }
        string Name { get; set; }
        IReadOnlyDictionary<string, BeastResistance> Resistances { get; }
        IReadOnlyCollection<SkillTemplate> Skills { get; }
        SpeciesType Species { get; set; }
        IReadOnlyCollection<SpellTemplate> Spells { get; }
        string Traits { get; set; }
        Die WillPower { get; set; }

        Die GetDie(string attributeName);
    }
}