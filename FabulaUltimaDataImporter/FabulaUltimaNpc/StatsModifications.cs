namespace FabulaUltimaNpc
{
    public class StatsModifications
    {
        public int InitiativeModifier { get; set; }
        public int MagicDefenseModifier { get; set; }
        public int DefenseModifier { get; set; }
        public bool DefenseOverrides { get; set; }

        internal StatsModifications Clone()
        {
            return new StatsModifications
            {
                InitiativeModifier = InitiativeModifier,
                MagicDefenseModifier = MagicDefenseModifier,
                DefenseModifier = DefenseModifier,
                DefenseOverrides = DefenseOverrides
            };
        }
    }
}
