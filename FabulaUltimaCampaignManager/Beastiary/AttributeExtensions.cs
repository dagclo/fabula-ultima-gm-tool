using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstProject.Beastiary
{
    public static class AttributeExtensions
    {
        public static string ShortenAttribute(this string attributeFullName)
        {
            // todo: switch all references to use this
            var normalizedName = attributeFullName.ToUpperInvariant();
            switch(normalizedName) 
            {
                case INSIGHT:
                    return INSIGHT_SHORT_NAME;
                case DEXTERITY:
                    return DEXTERITY_SHORT_NAME;
                case MIGHT:
                    return MIGHT_SHORT_NAME;
                case WILLPOWER:
                    return WILLPOWER_SHORT_NAME;
                case "HEALTHPOINTS":
                case "HEALTH POINTS":
                    return "HP";
                case "MAGICPOINTS":
                case "MAGIC POINTS":
                    return "MP";
                case "INITIATIVE":
                    return "INIT";
                case "DEFENSE":
                    return "DEF";
                case "MAGICDEFENSE":
                case "MAGIC DEFENSE":
                    return "M.DEF";
                default:
                    throw new ArgumentException($"{attributeFullName} not supported", nameof(attributeFullName));
            }
        }

        public static string LengthenAttributeName(this string attributeShortName)
        {
            // todo: switch all references to use this
            var normalizedName = attributeShortName.ToUpperInvariant();
            switch (normalizedName)
            {
                case INSIGHT_SHORT_NAME:
                    return INSIGHT;
                case DEXTERITY_SHORT_NAME:
                    return DEXTERITY;
                case MIGHT_SHORT_NAME:
                    return MIGHT;
                case WILLPOWER_SHORT_NAME:
                    return WILLPOWER;
                default:
                    throw new ArgumentException($"{attributeShortName} not supported", nameof(attributeShortName));
            }
        }

        public const string DEXTERITY = "DEXTERITY";
        public const string DEXTERITY_SHORT_NAME = "DEX";
        public const string MIGHT = "MIGHT";
        public const string MIGHT_SHORT_NAME = "MIG";
        public const string INSIGHT = "INSIGHT";
        public const string INSIGHT_SHORT_NAME = "INS";
        public const string WILLPOWER = "WILLPOWER";
        public const string WILLPOWER_SHORT_NAME = "WLP";

        public static IEnumerable<string> AttributeNames => new[] {DEXTERITY, MIGHT, INSIGHT, WILLPOWER};
        public static IEnumerable<string> ShortAttributeNames => AttributeNames.Select(n => n.ShortenAttribute());

    }
}
