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

        public IReadOnlyDictionary<string, BeastResistance> Resistances => Model.Resistances;
        public IReadOnlyCollection<BasicAttackTemplate> BasicAttacks => Model.BasicAttacks;
        public IReadOnlyCollection<SpellTemplate> Spells => Model.Spells;
        public IReadOnlyCollection<EquipmentTemplate> Equipment => Model.Equipment;
        public IReadOnlyCollection<SkillTemplate> Skills => Model.Skills;
        public IReadOnlyCollection<ActionTemplate> Actions => Model.Actions;

        // calculated

        public int MagicalDefense => Insight.Sides;
        public int Defense => Dexterity.Sides;

        public int Initiative => (Dexterity.Sides + Insight.Sides) / 2 + (Equipment?.Sum(e => e.StatsModifier?.InitiativeModifier ?? 0) ?? 0) + Model.Rank.GetNumSoldiersReplaced();
        public int HealthPoints => (2 * Level + 5 * Might.Sides) * Model.Rank.GetNumSoldiersReplaced();
        public int MagicPoints => (Level + 5 * WillPower.Sides) * Model.Rank.MagicPointMultiplier();
        public int Crisis => HealthPoints / 2;


        public IEnumerable<BasicAttackTemplate> AllAttacks =>
            (BasicAttacks ?? Enumerable.Empty<BasicAttackTemplate>())
            .Concat(Equipment?.Select(e => e.BasicAttack).Where(a => a != null) ??
                Enumerable.Empty<BasicAttackTemplate>());

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
    }
}
