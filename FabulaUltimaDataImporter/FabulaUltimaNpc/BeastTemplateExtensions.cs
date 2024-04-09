using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FabulaUltimaNpc
{
    public static class BeastTemplateExtensions
    {
        public static string GetAttributeValue(this IBeastTemplate template, string attributeName)
        {
            switch (attributeName)
            {
                case nameof(IBeastTemplate.Name):
                    return template.Name;
                case nameof(IBeastTemplate.Level):
                    return template.Level.ToString();
                case nameof(IBeastTemplate.Traits):
                    return template.Traits;
                case nameof(IBeastTemplate.Species):
                    return template.Species.Name;
                case nameof(IBeastTemplate.Description):
                    return template.Description;
                case nameof(IBeastTemplate.Dexterity):
                    return template.Dexterity.ToString();
                case nameof(IBeastTemplate.WillPower):
                    return template.WillPower.ToString();
                case nameof(IBeastTemplate.Might):
                    return template.Might.ToString();
                case nameof(IBeastTemplate.Insight):
                    return template.Insight.ToString();
                case nameof(IBeastTemplate.Initiative):
                    return template.Initiative.ToString();
                case nameof(IBeastTemplate.MagicalDefense):
                    return template.MagicalDefense.ToString();
                case nameof(IBeastTemplate.Defense):
                    return template.Defense.ToString();
                default:
                    throw new ArgumentException($"unknown attribute {attributeName}", nameof(attributeName));
            }
        }
    }
}
