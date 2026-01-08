using System.Text;

namespace FabulaUltimaNpc
{

    public class BeastTemplate : IBeastTemplate
    {
        public IBeast Model { get; }

        public BeastTemplate(IBeast beast)
        {
            Model = beast;
        }

        public Guid Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
            }
        }
        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
            }
        }
        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
            }
        }
        public int Level
        {
            get
            {
                return Model.Level;
            }
            set
            {
                Model.Level = value;
            }
        }
        public string Traits
        {
            get
            {
                return Model.Traits;
            }
            set
            {
                Model.Traits = value;
            }
        }
        public SpeciesType Species
        {
            get
            {
                return Model.Species;
            }
            set
            {
                Model.Species = value;
            }
        }
        public Die Insight
        {
            get
            {
                return Model.Insight;
            }
            set
            {
                Model.Insight = value;
            }
        }
        public Die Dexterity
        {
            get
            {
                return Model.Dexterity;
            }
            set
            {
                Model.Dexterity = value;
            }
        }
        public Die Might
        {
            get
            {
                return Model.Might;
            }
            set
            {
                Model.Might = value;
            }
        }
        public Die WillPower
        {
            get
            {
                return Model.WillPower;
            }
            set
            {
                Model.WillPower = value;
            }
        }
        public string ImageFile
        {
            get
            {
                return Model.ImageFile;
            }
            set
            {
                Model.ImageFile = value;
            }
        }

        public IReadOnlyDictionary<string, BeastResistance> Resistances => Model.Resistances as IReadOnlyDictionary<string, BeastResistance>;
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks => Model.BasicAttacks as IReadOnlyCollection<BasicAttackTemplate>;
        public IReadOnlyCollection<SpellTemplate> Spells => Model.Spells as IReadOnlyCollection<SpellTemplate>;
        public IReadOnlyCollection<EquipmentTemplate> Equipment => Model.Equipment as IReadOnlyCollection<EquipmentTemplate>;
        public IReadOnlyCollection<SkillTemplate> Skills => Model.Skills as IReadOnlyCollection<SkillTemplate>;
        public IReadOnlyCollection<ActionTemplate> Actions => Model.Actions as IReadOnlyCollection<ActionTemplate>;

        // calculated

        public int MagicalDefense => Insight.Sides;
        public int Defense => Dexterity.Sides;

        public int Initiative => (Dexterity.Sides + Insight.Sides) / 2 + (Equipment?.Sum(e => e.StatsModifier?.InitiativeModifier ?? 0) ?? 0) + (Model.Rank.GetNumSoldiersReplaced() - 1);
        public int HealthPoints => (2 * Level + 5 * Might.Sides) * Model.Rank.GetNumSoldiersReplaced();
        public int MagicPoints => (Level + 5 * WillPower.Sides) * Model.Rank.MagicPointMultiplier();
        public int Crisis => HealthPoints / 2;


#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        public IEnumerable<BasicAttackTemplate> AllAttacks =>
            (BasicAttacks ?? Enumerable.Empty<BasicAttackTemplate>())
            .Concat(Equipment?.Select(static e => e.BasicAttack).Where(static a => a != null) ??
                []);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.


        public int LevelDamageModifier
        {
            get
            {
                if (Level >= 60) return 15;
                if (Level >= 40) return 10;
                if (Level >= 20) return 5;
                return 0;
            }
        }

        public int LevelAccuracyModifier => Level / 10;

        public int MagicCheckModifier => LevelAccuracyModifier;

        public bool CanBeModified => true;
        public bool CanBeDeleted => true;

        public bool HasDefenseOverride => false;

        public Die GetDie(string attributeName)
        {
            switch (attributeName)
            {
                case nameof(Dexterity):
                    return Dexterity;
                case nameof(WillPower):
                    return WillPower;
                case nameof(Might):
                    return Might;
                case nameof(Insight):
                    return Insight;
                default:
                    throw new ArgumentException($"unknown attribute {attributeName}", nameof(attributeName));
            }
        }

        public string ToText()
        {
            var result = new StringBuilder();
            result.AppendLine($"{Name.ToUpperInvariant()}   Lv {Level} - {Species.Name.ToUpperInvariant()}");
            result.AppendLine();
            result.AppendLine($"{Description}");
            return result.ToString();
        }
    }
}
