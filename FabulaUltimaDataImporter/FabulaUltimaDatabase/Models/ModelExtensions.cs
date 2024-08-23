using FabulaUltimaNpc;
using Newtonsoft.Json;

namespace FabulaUltimaDatabase.Models
{
    public static class ModelExtensions
    {
        public static Die ToDie(this int? numSides)
        {
           
            return numSides.HasValue ? new Die(numSides.Value) : throw new ArgumentNullException();
        }

        public static SpeciesType ToSpeciesType(this Species species)
        {
            return new SpeciesType(species.Id, species.Name);
        }

        public static SkillEntry ToSkillEntry(this SkillTemplate skillTemplate)
        {
            return new SkillEntry
            {
                Id = skillTemplate.Id.ToString(),
                Name = skillTemplate.Name,
                TargetType = skillTemplate.TargetType.ToString(),
                Text = skillTemplate.Text ?? string.Empty,
                IsSpecialRule = skillTemplate.IsSpecialRule ? 1 : 0,                
                Keywords = Newtonsoft.Json.JsonConvert.SerializeObject(skillTemplate.Keywords?.ToArray()),
                OtherAttributes = skillTemplate.OtherAttributes != null ? Newtonsoft.Json.JsonConvert.SerializeObject(skillTemplate.OtherAttributes.DataDictionary) : null,
            };
        }

        public static SkillTemplate ToSkillTemplate(this SkillEntry skillEntry) 
        {
            var skillAttributes = skillEntry.OtherAttributes == null ? new SkillAttributeCollection() : 
                new SkillAttributeCollection(JsonConvert.DeserializeObject<Dictionary<string, string>>(skillEntry.OtherAttributes));

            return new SkillTemplate(Guid.Parse(skillEntry.Id))
            {
                Name = skillEntry.Name,
                IsSpecialRule = skillEntry.IsSpecialRule == 0 ? false : true,
                Keywords = skillEntry.Keywords == null ? new HashSet<string>() : JsonConvert.DeserializeObject<HashSet<string>>(skillEntry.Keywords),
                TargetType = Type.GetType(skillEntry.TargetType),
                Text = skillEntry.Text ?? string.Empty,
                OtherAttributes = skillAttributes,                

            };
        }

        public static DamageType ToDamageType(this DamageTypeEntry damageTypeEntry)
        {
            return new DamageType()
            {
                Id = damageTypeEntry.Id,
                Name = damageTypeEntry.Name,
            };
        }
    }
}
