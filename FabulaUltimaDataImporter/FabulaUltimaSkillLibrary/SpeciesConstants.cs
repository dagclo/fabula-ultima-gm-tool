namespace FabulaUltimaSkillLibrary
{
    public static class SpeciesConstants
    {
        public const string BLOCKED_SPECIES = "BlockedSpecies";
        public const string FREE_SPECIES = "FreeSpecies";
        public const string IS_SPECIES_SKILL = "IsSpeciesSkill";
        public const string SPECIES_VULNERABILITY_CHOICE = "SpeciesVulnerabilityChoice";
        public static readonly Guid BEAST = Guid.Parse("b0788720-8fa0-4968-ac61-5f3063d97c17");
        public static readonly Guid HUMANOID = Guid.Parse("69711547-14c6-4a01-af94-f5d5117a6bae");
        public static readonly Guid CONSTRUCT = Guid.Parse("f50815fc-9d41-4eeb-9797-182544244f0a");
        public static readonly Guid DEMON = Guid.Parse("37e76b06-fd97-4c73-8509-eb42e3610eef");
        public static readonly Guid ELEMENTAL = Guid.Parse("19014999-30a7-4635-b1a1-505b10a5bc19");
        public static readonly Guid MONSTER = Guid.Parse("23e74a9c-8413-497f-b098-f541b43884c0");
        public static readonly Guid PLANT = Guid.Parse("d608585c-32ff-4d10-88b9-b4df66364195");
        public static readonly Guid UNDEAD = Guid.Parse("3e35bbec-d713-4efc-af8a-3d5e01403885");

        public static Guid FromString(string species)
        {
            var key = species.ToUpperInvariant();
            switch(key)
            {
                case nameof(BEAST):
                    return BEAST;
                case nameof(HUMANOID):
                    return HUMANOID;
                case nameof(CONSTRUCT):
                    return CONSTRUCT;
                case nameof(DEMON):
                    return DEMON;
                case nameof(ELEMENTAL):
                    return ELEMENTAL;
                case nameof(MONSTER):
                    return MONSTER;
                case nameof(PLANT):
                    return PLANT;
                case nameof(UNDEAD):
                    return UNDEAD;
                default:
                    throw new ArgumentException($"invalid species '{species}'");
            }
        }
    }
}
