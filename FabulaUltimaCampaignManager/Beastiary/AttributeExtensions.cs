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
                    return "INS";
                case DEXTERITY:
                    return "DEX";
                case MIGHT:
                    return "MIG";
                case WILLPOWER:
                    return "WLP";
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

        public const string DEXTERITY = "DEXTERITY";
        public const string MIGHT = "MIGHT";
        public const string INSIGHT = "INSIGHT";
        public const string WILLPOWER = "WILLPOWER";

        public static IEnumerable<string> AttributeNames => new[] {DEXTERITY, MIGHT, INSIGHT, WILLPOWER};
        public static IEnumerable<string> ShortAttributeNames => AttributeNames.Select(n => n.ShortenAttribute());

    }
}
