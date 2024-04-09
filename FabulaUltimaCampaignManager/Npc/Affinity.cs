using FabulaUltimaNpc;
using System;

namespace FirstProject.Npc
{
    public enum Affinity
    {
        NONE = 0,
        VULNERABLE,
        ABSORBS,
        IMMUNE,
        RESISTANT,
        HEAL // special affinity
    }

    public static class AffinityExtensions
    {
        public static Affinity ToAffinity(this BeastResistance beastResistance)
        {
            switch (beastResistance.Affinity.ToString())
            {
                case "RS":
                    return Affinity.RESISTANT;
                case "VU":
                    return Affinity.VULNERABLE;
                case "AB":
                    return Affinity.ABSORBS;
                case "IM":
                    return Affinity.IMMUNE;
                case "":
                    return Affinity.NONE;
                default:
                    throw new ArgumentException(nameof(beastResistance));
            }

        }
    }
}
