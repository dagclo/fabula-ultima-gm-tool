namespace FabulaUltimaNpc
{
    public enum Rank
    {
        Soldier = 1,
        Elite,
        Champion,
        Super_Champion,
        Uber_Champion
    }

    public static class InstanceRankExtensions
    {
        public static int GetNumSkills(this Rank instanceRank)
        {
            switch (instanceRank)
            {
                case Rank.Soldier:
                    return 0;
                case Rank.Elite:
                    return 1;
                case Rank.Champion:
                    return 1;
                case Rank.Super_Champion:
                    return 2;
                case Rank.Uber_Champion:
                    return 3;
                default:
                    throw new ArgumentException(nameof(instanceRank));
            }
        }

        public static int GetNumSoldiersReplaced(this Rank instanceRank)
        {
            switch (instanceRank)
            {
                case Rank.Soldier:
                    return 1;
                case Rank.Elite:
                    return 2;
                case Rank.Champion:
                    return 2;
                case Rank.Super_Champion:
                    return 3;
                case Rank.Uber_Champion:
                    return 4;
                default:
                    throw new ArgumentException(nameof(instanceRank));
            }
        }
        public static int MagicPointMultiplier(this Rank instanceRank)
        {
            switch (instanceRank)
            {
                case Rank.Soldier:                    
                case Rank.Elite:
                    return 1;
                case Rank.Champion:                    
                case Rank.Super_Champion:                    
                case Rank.Uber_Champion:
                    return 2;
                default:
                    throw new ArgumentException(nameof(instanceRank));
            }
        }
    }
}
