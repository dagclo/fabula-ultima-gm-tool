using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Npc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public static class FultimatorHelper
    {
        public static bool TryParseJson(string json, out Entry result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<Entry>(json);
                return true;
            }
            catch(Exception)
            {
                result = null;
                return false;
            }
        }

        public static IBeastTemplate AsBeastTemplate(this Entry entry)
        {
            return new BeastTemplate(entry as IBeast);
        }
    }

    public class Entry: IBeast
    {
        [JsonProperty("extra")]
        public Extra extra { get; set; }
        [JsonProperty("language")]
        public string language { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("affinities")]
        public Affinities Affinities { get; set; }
        [JsonProperty("published")]
        public bool published { get; set; }
        [JsonProperty("publishedAt")]
        public long publishedAt { get; set; }
        [JsonProperty("rank")]
        public string Rank { get; set; }
        [JsonProperty("special")]
        public List<Special> special { get; set; }
        [JsonProperty("traits")]
        public string Traits { get; set; }
        [JsonProperty("searchString")]
        public List<string> searchString { get; set; }
        [JsonProperty("createdBy")]
        public string createdBy { get; set; }
        [JsonProperty("spells")]
        public List<Spell> spells { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("name")]
        public string uid { get; set; }
        [JsonProperty("villain")]
        public string villain { get; set; }
        [JsonProperty("lvl")]
        public int Level { get; set; }
        [JsonProperty("attacks")]
        public List<Attack> attacks { get; set; }
        [JsonProperty("species")]
        public string Species { get; set; }
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        Guid IBeast.Id { get; set; } = Guid.NewGuid();
        string IBeast.Name { get => this.Name; set => this.Name = value; }
        string IBeast.Description { get => this.Description; set => this.Description = value; }
        int IBeast.Level { get => this.Level; set => this.Level = value; }
        string IBeast.Traits { get => this.Traits; set => this.Traits = value; }
        SpeciesType IBeast.Species
        {
            get
            {
                var checkValue = this.Species.ToUpperInvariant();
                Guid speciesId;
                switch(checkValue)
                {
                    case "BEAST":
                        speciesId = SpeciesConstants.BEAST;
                        break;
                    case "CONSTRUCT":
                        speciesId = SpeciesConstants.CONSTRUCT;
                        break;
                    case "DEMON":
                        speciesId = SpeciesConstants.DEMON;
                        break;
                    case "ELEMENTAL":
                        speciesId = SpeciesConstants.ELEMENTAL;
                        break;
                    case "HUMANOID":
                        speciesId = SpeciesConstants.HUMANOID;
                        break;
                    case "MONSTER":
                        speciesId = SpeciesConstants.MONSTER;
                        break;
                    case "PLANT":
                        speciesId = SpeciesConstants.PLANT;
                        break;
                    case "UNDEAD":
                        speciesId = SpeciesConstants.UNDEAD;
                        break;
                    default:
                        throw new InvalidSpeciesException(this.Species);
                }
                return new SpeciesType(speciesId, checkValue);
            }
            set
            {
                this.Species = value.Name;
            }
        }
        Die IBeast.Insight { get => new Die(this.Attributes.Insight); set => this.Attributes.Insight = value.Sides; }
        Die IBeast.Dexterity { get => new Die(this.Attributes.Dexterity); set => this.Attributes.Dexterity = value.Sides; }
        Die IBeast.Might { get => new Die(this.Attributes.Might); set => this.Attributes.Might = value.Sides; }
        Die IBeast.WillPower { get => new Die(this.Attributes.Willpower); set => this.Attributes.Willpower = value.Sides; }
        string IBeast.ImageFile { get; set; }

        FabulaUltimaNpc.Rank IBeast.Rank => Enum.TryParse<FabulaUltimaNpc.Rank>(this.Rank, out var fabulaRank) ? fabulaRank : throw new InvalidRankException(this.Rank);

        IReadOnlyDictionary<string, BeastResistance> IBeast.Resistances
        {
            get
            {
                Guid GetAffinityId(string value)
                {
                    switch (value.ToUpperInvariant())
                    {
                        case "RS":
                            return DamageConstants.RESISTANT;
                        case "VU":
                            return DamageConstants.VULNERABLE;
                        case "AB":
                            return DamageConstants.ABSORBS;
                        case "IM":
                            return DamageConstants.IMMUNE;
                        case "":
                            return DamageConstants.NO_AFFINITY;
                        default:
                            throw new ArgumentException(value);
                    }

                }

                var result = new Dictionary<string, BeastResistance>
                {
                    { 
                        DamageConstants.PHYSICAL_NAME.ToLowerInvariant().FirstCharToUpper(), 
                        new BeastResistance
                        { 
                            DamageTypeId = DamageConstants.PHYSICAL_DAMAGE_TYPE, 
                            DamageType = DamageConstants.PHYSICAL_NAME,
                            Affinity = this.Affinities.Physical ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Physical ?? "")
                        } 
                    },
                    {
                        DamageConstants.AIR_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.AIR_DAMAGE_TYPE,
                            DamageType = DamageConstants.AIR_NAME,
                            Affinity = this.Affinities.Air ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Air ?? "")
                        }
                    },
                    {
                        DamageConstants.BOLT_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.BOLT_DAMAGE_TYPE,
                            DamageType = DamageConstants.BOLT_NAME,
                            Affinity = this.Affinities.Bolt ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Bolt ?? "")
                        }
                    },
                    {
                        DamageConstants.DARK_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.DARK_DAMAGE_TYPE,
                            DamageType = DamageConstants.DARK_NAME,
                            Affinity = this.Affinities.Dark ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Dark ?? "")
                        }
                    },
                    {
                        DamageConstants.EARTH_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.EARTH_DAMAGE_TYPE,
                            DamageType = DamageConstants.EARTH_NAME,
                            Affinity = this.Affinities.Earth ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Earth ?? "")
                        }
                    },
                    {
                        DamageConstants.FIRE_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.FIRE_DAMAGE_TYPE,
                            DamageType = DamageConstants.FIRE_NAME,
                            Affinity = this.Affinities.Fire ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Fire ?? "")
                        }
                    },
                    {
                        DamageConstants.ICE_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.ICE_DAMAGE_TYPE,
                            DamageType = DamageConstants.ICE_NAME,
                            Affinity = this.Affinities.Ice ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Ice ?? "")
                        }
                    },
                    {
                        DamageConstants.POISON_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.POISON_DAMAGE_TYPE,
                            DamageType = DamageConstants.POISON_NAME,
                            Affinity = this.Affinities.Poison ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Poison ?? "")
                        }
                    },
                    {
                        DamageConstants.LIGHT_NAME.ToLowerInvariant().FirstCharToUpper(),
                        new BeastResistance
                        {
                            DamageTypeId = DamageConstants.LIGHT_DAMAGE_TYPE,
                            DamageType = DamageConstants.LIGHT_NAME,
                            Affinity = this.Affinities.Light ?? "",
                            AffinityId = GetAffinityId(this.Affinities.Light ?? "")
                        }
                    },
                };
                return result;
            }
        }

        IReadOnlyCollection<BasicAttackTemplate> IBeast.BasicAttacks => throw new NotImplementedException();

        IReadOnlyCollection<SpellTemplate> IBeast.Spells => throw new NotImplementedException();

        IReadOnlyCollection<EquipmentTemplate> IBeast.Equipment => throw new NotImplementedException();

        IReadOnlyCollection<SkillTemplate> IBeast.Skills => throw new NotImplementedException();

        IReadOnlyCollection<ActionTemplate> IBeast.Actions => throw new NotImplementedException();
    }
}
