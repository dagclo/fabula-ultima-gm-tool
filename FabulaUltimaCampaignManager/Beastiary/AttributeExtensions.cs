using System;

namespace FirstProject.Beastiary
{
    public static class AttributeExtensions
    {
        public static string ShortenAttribute(this string attributeFullName)
        {
            // todo: switch all references to use this
            var normalizedName = attributeFullName.ToLowerInvariant();
            switch(normalizedName) 
            {
                case "insight":
                    return "INS";
                case "dexterity":
                    return "DEX";
                case "might":
                    return "MIG";
                case "willpower":
                    return "WLP";
                case "healthpoints":
                case "health points":
                    return "HP";
                case "magicpoints":
                case "magic points":
                    return "MP";
                case "initiative":
                    return "INIT";
                case "defense":
                    return "DEF";
                case "magicdefense":
                case "magic defense":
                    return "M.DEF";
                default:
                    throw new ArgumentException($"{attributeFullName} not supported", nameof(attributeFullName));
            }
        }
    }
}
