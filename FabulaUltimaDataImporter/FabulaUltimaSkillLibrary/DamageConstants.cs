namespace FabulaUltimaSkillLibrary
{
    public static class DamageConstants
    {
        public const string AFFINITY_ID = "AffinityId";
        public const string AFFINITY_TRUMPS = "AffinityTrumpList";
        public readonly static Guid NO_AFFINITY = Guid.Parse("77a9c176-4ddb-46ef-a945-0023bf7a6f6b");
        public readonly static Guid VULNERABLE  = Guid.Parse("fc15b76a-6505-46f4-81ad-da4a5bf1bd42");
        public readonly static Guid RESISTANT   = Guid.Parse("75059ae9-e6c7-4d45-a1f1-18bf617b0d2a");
        public readonly static Guid IMMUNE      = Guid.Parse("1d843905-9d75-4c57-84c2-07dc7d179d3b");
        public readonly static Guid ABSORBS     = Guid.Parse("40058e84-b555-4943-bf4d-5c7381272749");

        public const string DAMAGE_TYPE_ID = "DamageTypeId";
        public static readonly Guid DARK_DAMAGE_TYPE     = Guid.Parse("c635cee1-fcbc-44cd-98c9-fe55c7084806");
        public static readonly Guid EARTH_DAMAGE_TYPE    = Guid.Parse("813f85c6-fa28-42f2-ad4b-b682a4814382");
        public static readonly Guid FIRE_DAMAGE_TYPE     = Guid.Parse("dda401cf-437c-438e-9a1b-e3421f9c4902");
        public static readonly Guid ICE_DAMAGE_TYPE      = Guid.Parse("01a0f627-748c-49eb-999f-03746b673be5");
        public static readonly Guid AIR_DAMAGE_TYPE      = Guid.Parse("7dfe93bd-67d5-468d-bb32-b4d8c1676305");
        public static readonly Guid PHYSICAL_DAMAGE_TYPE = Guid.Parse("ffad483e-0ad5-4a43-b235-080ddfd67470");
        public static readonly Guid BOLT_DAMAGE_TYPE     = Guid.Parse("39fb0c13-06df-47e0-ae4b-ccfb2012b03d");
        public static readonly Guid LIGHT_DAMAGE_TYPE    = Guid.Parse("9ef9cb1e-96da-4acc-ae0e-66e8e5236888");
        public static readonly Guid POISON_DAMAGE_TYPE   = Guid.Parse("f36c11bf-a896-4cc7-9460-37bf5100e14a");

        public const string DAMAGE_BOOST = "DamageBoost";

        public const string DAMAGE_TYPE_NAME = "DamageTypeName";
        public const string DARK_NAME = "dark";
        public const string EARTH_NAME = "earth";
        public const string FIRE_NAME = "fire";
        public const string ICE_NAME = "ice";
        public const string AIR_NAME = "air";
        public const string PHYSICAL_NAME = "physical";
        public const string BOLT_NAME = "bolt";
        public const string LIGHT_NAME = "light";
        public const string POISON_NAME = "poison";

        public static IDictionary<Guid, string> AffinityMap { get; } = new Dictionary<Guid, string>()
        {
            { NO_AFFINITY, "" },
            { VULNERABLE, "VU" },
            { RESISTANT, "RS" },
            { IMMUNE, "IM" },
            { ABSORBS, "AB" },
        };

        public static IDictionary<string, Guid> DamageTypeMap { get; } = new Dictionary<string, Guid>()
        {
            { DARK_NAME, DARK_DAMAGE_TYPE },
            { EARTH_NAME, EARTH_DAMAGE_TYPE },
            { FIRE_NAME, FIRE_DAMAGE_TYPE },
            { ICE_NAME, ICE_DAMAGE_TYPE },
            { AIR_NAME, AIR_DAMAGE_TYPE },
            { PHYSICAL_NAME, PHYSICAL_DAMAGE_TYPE },
            { BOLT_NAME, BOLT_DAMAGE_TYPE },
            { LIGHT_NAME, LIGHT_DAMAGE_TYPE },
            { POISON_NAME, POISON_DAMAGE_TYPE },
        };
    }
}
