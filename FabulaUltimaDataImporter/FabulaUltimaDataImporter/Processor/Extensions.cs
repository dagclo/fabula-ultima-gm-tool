using FabulaUltimaDataImporter.IO;
using FabulaUltimaNpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaDataImporter.Processor
{
    public static class Extensions
    {
        private static IReadOnlySet<string> ATTRIBUTE_VALUES = new HashSet<string>()
        {
            nameof(IBeastTemplate.Insight),
            nameof(IBeastTemplate.Might),
            nameof(IBeastTemplate.Dexterity),
            nameof(IBeastTemplate.WillPower),
        };

        public static string GetAttribute(this UserIOWrapper userIO, int number, bool allowNull = false)
        {
            (bool verified, string error) OnlyAllowAttributeNames(string arg)
            {
                if (allowNull && string.IsNullOrWhiteSpace(arg))
                {
                   return (true, string.Empty);
                }
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (!ATTRIBUTE_VALUES.Contains(arg))
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };
            userIO.WriteLine($"Choose a attribute ({string.Join(", ", ATTRIBUTE_VALUES)})");
            return userIO.GetValidString($"Attribute{number}", additionalVerification: OnlyAllowAttributeNames, allowEmpty: allowNull);
        }
    }
}
