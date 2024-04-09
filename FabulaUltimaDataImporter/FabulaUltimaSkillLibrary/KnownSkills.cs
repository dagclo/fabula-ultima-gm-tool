using static FabulaUltimaSkillLibrary.DamageConstants;
using static FabulaUltimaSkillLibrary.SpeciesConstants;
using static FabulaUltimaSkillLibrary.StatsConstants;
using static FabulaUltimaSkillLibrary.CheckConstants;
using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibrary
{

    public static class KnownSkills
    {
        #region Special Attacks

        public const string IS_SPECIAL_ATTACK = nameof(SkillAttributeCollection.IsSpecialAttack);
        public const string IS_SPECIAL_ATTACK_DETRIMENT = "IsDetrimentalSpecialAttack";

        public static readonly SkillTemplate SpecialAttackSufferWeak = 
            new SkillTemplate(Guid.Parse("e26ad64f-698f-46ce-967d-49d10cbe4664"))
        {

            Name = "Special Attack: Impose Weak",
            TargetType = typeof(BasicAttackTemplate),
            Text = "the target suffers weak",
            IsSpecialRule = false,
            Keywords = new HashSet<string> { "weak", "suffers" },
            OtherAttributes = new SkillAttributeCollection
            {
                [IS_SPECIAL_ATTACK] = true.ToString(),
                [IS_KNOWN_SKILL] = true.ToString(),
            }
        };

        public static readonly SkillTemplate SpecialAttackSufferSlow =
            new SkillTemplate(Guid.Parse("c0646563-720b-44da-a1f9-6bd8b3051d9d"))
            {

                Name = "Special Attack: Impose Slow",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target suffers slow",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "slow", "suffers" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackSufferShaken =
            new SkillTemplate(Guid.Parse("971e9ccd-d2e1-46d8-8cf0-5e65c0e8e17f"))
            {

                Name = "Special Attack: Impose Shaken",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target suffers shaken",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "shaken", "suffers" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackSufferDazed =
            new SkillTemplate(Guid.Parse("80f7996d-606d-483b-bdfc-d37c753a213c"))
            {

                Name = "Special Attack: Impose Dazed",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target suffers dazed",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "dazed", "suffers" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackSufferEnraged =
            new SkillTemplate(Guid.Parse("75b50aaf-944a-4b01-9dba-cfaa53aa3b47"))
            {

                Name = "Special Attack: Impose Enraged",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target suffers enraged",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "enraged", "suffers" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackSufferPoisoned =
            new SkillTemplate(Guid.Parse("c7804bac-7173-431f-b5fe-0ff9d173a28c"))
            {

                Name = "Special Attack: Impose Poisoned",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target suffers poisoned",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "poisoned", "suffers" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackOnGuardExtraDamage =
            new SkillTemplate(Guid.Parse("77d75928-3570-4b41-b48b-9fee1c490b57"))
            {

                Name = "Special Attack: When Guarding, Extra damage",
                TargetType = typeof(BasicAttackTemplate),
                Text = "If creature performed the Guard action during its previous turn, this attack deals 5 extra damage",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "guard", },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackTwoTargets =
            new SkillTemplate(Guid.Parse("C521419C-1D17-4106-96E2-9E2701F941CB"))
            {

                Name = "Special Attack: Attack Two Targets",
                TargetType = typeof(BasicAttackTemplate),
                Text = "This attack has multi (2)",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "multi", "(2)", },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackTargetMagicDefense =
            new SkillTemplate(Guid.Parse("407ec320-142f-4af7-8ce2-c4045eff7eb1"))
            {

                Name = "Special Attack: Target Magic Defense",
                TargetType = typeof(BasicAttackTemplate),
                Text = "This attack targets Magic Defense",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "targets", "Magic", "defense" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackPreventObjectiveAction =
            new SkillTemplate(Guid.Parse("44b145c9-ab03-4a1b-a677-14919892bf43"))
            {

                Name = "Special Attack: Prevent Objective Action",
                TargetType = typeof(BasicAttackTemplate),
                Text = "the target cannot perform the Objective action on their next turn",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "objective", "action", "next", "turn" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackExtraDamageOnSlow =
            new SkillTemplate(Guid.Parse("4082bbf4-dc46-4078-b213-3bc3934f2d05"))
            {

                Name = "Special Attack: Extra Damage on Slow",
                TargetType = typeof(BasicAttackTemplate),
                Text = "This attack deals 5 extra damage against slow targets",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "slow", "extra", "damage" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackExtraDamageOnShaken =
            new SkillTemplate(Guid.Parse("5c07e3a6-c806-467c-af4a-4ed00a222e21"))
            {

                Name = "Special Attack: Extra Damage on Shaken",
                TargetType = typeof(BasicAttackTemplate),
                Text = "This attack deals 5 extra damage against shaken targets",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "shaken", "extra", "damage" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpecialAttackAltDamageIce =
            new SkillTemplate(Guid.Parse("3c6e44eb-3f65-45ff-bfc5-f419fa809bd9"))
            {

                Name = "Special Attack: Alternative Damage Ice",
                TargetType = typeof(BasicAttackTemplate),
                Text = "or ice damage",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "or", "ice", "damage" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [IS_SPECIAL_ATTACK] = true.ToString(),
                    [DAMAGE_TYPE_ID] = ICE_DAMAGE_TYPE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    Ordering = 0,
                }
            };

        #endregion

        #region Resistances, Vulnerabilities, Immunities

        public static readonly SkillTemplate FireVulnerability =
            new SkillTemplate(Guid.Parse("0599471f-6102-428b-9577-f72835db5e0d"))
            {
                Name = "Vulnerability: Fire",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "fire", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID]= VULNERABLE.ToString(),
                    [SPECIES_VULNERABILITY_CHOICE] = PLANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = FIRE_NAME,
                }
            };
        public static readonly SkillTemplate IceVulnerability =
            new SkillTemplate(Guid.Parse("2a5d9703-cc0c-4d92-a882-d22df8fe75e9"))
            {
                Name = "Vulnerability: Ice",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "ice", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [SPECIES_VULNERABILITY_CHOICE] = PLANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = ICE_NAME,
                }

            };

        public static readonly SkillTemplate PhysicalVulnerability =
            new SkillTemplate(Guid.Parse("b3d1ad55-7103-422f-bc0d-5c887d62b35d"))
            {
                Name = "Vulnerability: Physical",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "physical", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = PHYSICAL_NAME,
                }
            };
        public static readonly SkillTemplate BoltVulnerability =
            new SkillTemplate(Guid.Parse("4ed2e8d5-6470-4fda-9295-50216e861d68"))
            {
                Name = "Vulnerability: Bolt",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "bolt", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [SPECIES_VULNERABILITY_CHOICE] = PLANT.ToString(),
                    [DAMAGE_TYPE_NAME] = BOLT_NAME,
                }
            };
        public static readonly SkillTemplate AirVulnerability =
            new SkillTemplate(Guid.Parse("9b0f978b-5fcb-4b2c-afc8-37325a16ea66"))
            {
                Name = "Vulnerability: Air",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "air", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [SPECIES_VULNERABILITY_CHOICE] = PLANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = AIR_NAME,
                }
            };
        public static readonly SkillTemplate DarkVulnerability =
            new SkillTemplate(Guid.Parse("1972a425-9e64-4cbe-b4a0-b6a7d3b14f2d"))
            {
                Name = "Vulnerability: Dark",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "dark", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = DARK_NAME,
                }
            };
        public static readonly SkillTemplate EarthVulnerability =
            new SkillTemplate(Guid.Parse("47bedb9d-3b85-4406-8a57-29ed4709460e"))
            {
                Name = "Vulnerability: Earth",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "earth", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = EARTH_NAME,
                }
            };
        public static readonly SkillTemplate LightVulnerability =
            new SkillTemplate(Guid.Parse("ce9f749b-6212-4ce9-9ff7-97473c764402"))
            {
                Name = "Vulnerability: Light",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "light", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [SPECIES_VULNERABILITY_CHOICE] = UNDEAD.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = LIGHT_NAME,
                }
            };
        public static readonly SkillTemplate PoisonVulnerability =
            new SkillTemplate(Guid.Parse("8bac9477-fa24-4030-9042-f9ca700bb5a2"))
            {
                Name = "Vulnerability: Poison",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "poison", "vulnerability" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = VULNERABLE.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = POISON_NAME,
                }
            };

        public static readonly IDictionary<Guid, SkillTemplate> VulnerabilitySkills = new Dictionary<Guid, SkillTemplate>
        {
            { DamageConstants.FIRE_DAMAGE_TYPE, FireVulnerability },
            { DamageConstants.ICE_DAMAGE_TYPE, IceVulnerability },
            { DamageConstants.PHYSICAL_DAMAGE_TYPE, PhysicalVulnerability },
            { DamageConstants.BOLT_DAMAGE_TYPE, BoltVulnerability },
            { DamageConstants.AIR_DAMAGE_TYPE, AirVulnerability },
            { DamageConstants.DARK_DAMAGE_TYPE, DarkVulnerability },
            { DamageConstants.EARTH_DAMAGE_TYPE, EarthVulnerability },
            { DamageConstants.LIGHT_DAMAGE_TYPE, LightVulnerability },
            { DamageConstants.POISON_DAMAGE_TYPE, PoisonVulnerability },
        };


        internal static SkillTemplate GetVulnerabilitySkill(Guid damageTypeId)
        {
            return VulnerabilitySkills[damageTypeId];
        }

        public static readonly SkillTemplate DarkResistance =
            new SkillTemplate(Guid.Parse("686300cc-ab00-4646-9066-d4ad3217365e"))
            {
                Name = "Resistance: Dark",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "dark", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = DARK_NAME,
                }
            };

        public static readonly SkillTemplate EarthResistance =
            new SkillTemplate(Guid.Parse("68db6d04-e3fd-49dc-ae33-a54a176acf87"))
            {
                Name = "Resistance: Earth",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "earth", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [FREE_SPECIES] = CONSTRUCT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = EARTH_NAME,
                }
            };

        public static readonly SkillTemplate IceResistance =
            new SkillTemplate(Guid.Parse("a2892d5b-4391-4759-a495-0ff7eb5ca127"))
            {
                Name = "Resistance: Ice",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "ice", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = ICE_NAME,
                }
            };

        public static readonly SkillTemplate AirResistance =
            new SkillTemplate(Guid.Parse("88ec6ed6-560d-4749-823a-9b3a6445f93e"))
            {
                Name = "Resistance: Air",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "air", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = AIR_NAME,
                }
            };

        
        public static readonly SkillTemplate FireResistance =
            new SkillTemplate(Guid.Parse("da9894c6-d4fd-49d6-9764-672c5236845d"))
            {
                Name = "Resistance: Fire",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "fire", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = FIRE_NAME,
                }
            };

        public static readonly SkillTemplate PhysicalResistance =
            new SkillTemplate(Guid.Parse("db3c8f02-74c1-4b35-ab21-3988004e3bec"))
            {
                Name = "Resistance: physical",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "physical", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = PHYSICAL_NAME,
                }
            };

        public static readonly SkillTemplate BoltResistance =
            new SkillTemplate(Guid.Parse("8d5ef88d-1701-42c7-b103-7d48082a9bef"))
            {
                Name = "Resistance: Bolt",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "bolt", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = BOLT_NAME,
                }
            };

        public static readonly SkillTemplate LightResistance =
            new SkillTemplate(Guid.Parse("b8aef57a-860a-4c37-91ee-e2452de74183"))
            {
                Name = "Resistance: Light",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "light", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = LIGHT_NAME,
                }
            };

        public static readonly SkillTemplate PoisonResistance =
            new SkillTemplate(Guid.Parse("9ab3f45b-66cf-48a4-813e-174ded708c4b"))
            {
                Name = "Resistance: Poison",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "poison", "resistance" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = RESISTANT.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = POISON_NAME,
                }
            };

        public static readonly IDictionary<Guid, SkillTemplate> ResistanceSkills = new Dictionary<Guid, SkillTemplate>
        {
            { DamageConstants.DARK_DAMAGE_TYPE, DarkResistance },
            { DamageConstants.EARTH_DAMAGE_TYPE, EarthResistance },
            { DamageConstants.ICE_DAMAGE_TYPE, IceResistance },
            { DamageConstants.AIR_DAMAGE_TYPE, AirResistance },
            { DamageConstants.BOLT_DAMAGE_TYPE, BoltResistance },
            { DamageConstants.PHYSICAL_DAMAGE_TYPE, PhysicalResistance },
            { DamageConstants.LIGHT_DAMAGE_TYPE, LightResistance },
            { DamageConstants.FIRE_DAMAGE_TYPE, FireResistance },
            { DamageConstants.POISON_DAMAGE_TYPE, PoisonResistance },
        };

        internal static SkillTemplate GetResistanceSkill(Guid damageTypeId)
        {
            return ResistanceSkills[damageTypeId];
        }


        public static readonly SkillTemplate PoisonImmunity =
            new SkillTemplate(Guid.Parse("9901c2fa-44be-4359-8c01-c0544e882bd9"))
            {
                Name = "Immunity: Poison",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "poison", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    FreeSpecies = new[] { CONSTRUCT, ELEMENTAL, UNDEAD },
                    [DAMAGE_TYPE_NAME] = POISON_NAME,
                }
            };

        public static readonly SkillTemplate FireImmunity = 
            new SkillTemplate(Guid.Parse("01209421-703D-4D04-B7DC-51D8E48D6A64"))
            {
                Name = "Immunity: Fire",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "fire", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = FIRE_NAME,
                }
            };

        public static readonly SkillTemplate DarkImmunity =
            new SkillTemplate(Guid.Parse("84dabd33-811f-495d-be20-15ab10dea54f"))
            {
                Name = "Immunity: Dark",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "dark", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                            [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = DARK_NAME,
                    FreeSpecies = new[] { UNDEAD }
                        }
            };
        public static readonly SkillTemplate EarthImmunity =
            new SkillTemplate(Guid.Parse("da113fa6-544e-4b0c-af71-ef6f77f395a6"))
            {
                Name = "Immunity: Earth",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "earth", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = EARTH_NAME,
                }
            };
        public static readonly SkillTemplate IceImmunity =
            new SkillTemplate(Guid.Parse("b353377b-d527-4571-9e26-36edaf6f6494"))
            {
                Name = "Immunity: Ice",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "ice", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = ICE_NAME,
                }
            };
        public static readonly SkillTemplate AirImmunity =
            new SkillTemplate(Guid.Parse("972a6645-d32a-461f-9d93-d2e11fbee76a"))
            {
                Name = "Immunity: Air",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "air", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = AIR_NAME,
                }
            };
        public static readonly SkillTemplate BoltImmunity =
            new SkillTemplate(Guid.Parse("d95326b9-5276-4271-b2d3-b7ff45916452"))
            {
                Name = "Immunity: Bolt",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "bolt", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = BOLT_NAME,
                }
            };
        public static readonly SkillTemplate PhysicalImmunity =
            new SkillTemplate(Guid.Parse("b5375b1b-e5ab-4f68-a340-5b6161d5841e"))
            {
                Name = "Immunity: Physical",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "Physical", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = PHYSICAL_NAME,
                }
            };
        public static readonly SkillTemplate LightImmunity =
            new SkillTemplate(Guid.Parse("1be5a2a0-977c-4d0f-b4e2-c062b09fdf8f"))
            {
                Name = "Immunity: Light",
                TargetType = typeof(BeastResistance),
                IsSpecialRule = false,
                Keywords = new HashSet<string> { "light", "immunity" },
                OtherAttributes = new SkillAttributeCollection
                        {
                             [AFFINITY_ID] = IMMUNE.ToString() ,
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = LIGHT_NAME,
                }
            };
        public static readonly IDictionary<Guid, SkillTemplate> ImmunitySkills = new Dictionary<Guid, SkillTemplate>
        {
            { DamageConstants.DARK_DAMAGE_TYPE, DarkImmunity },
            { DamageConstants.EARTH_DAMAGE_TYPE, EarthImmunity },
            { DamageConstants.ICE_DAMAGE_TYPE, IceImmunity },
            { DamageConstants.AIR_DAMAGE_TYPE, AirImmunity },
            { DamageConstants.BOLT_DAMAGE_TYPE, BoltImmunity },
            { DamageConstants.PHYSICAL_DAMAGE_TYPE, PhysicalImmunity },
            { DamageConstants.LIGHT_DAMAGE_TYPE, LightImmunity },
            { DamageConstants.FIRE_DAMAGE_TYPE, FireImmunity },
            { DamageConstants.POISON_DAMAGE_TYPE, PoisonImmunity },
        };

        internal static SkillTemplate GetImmunitySkill(Guid damageTypeId)
        {
            return ImmunitySkills[damageTypeId];
        }

        #endregion

        #region Absorption
        public static readonly SkillTemplate DarkAbsorption =
            new SkillTemplate(Guid.Parse("042fa743-5af5-48a7-88ec-3d9b847ba87e"))
            {
                Name = "Absorption: Dark",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "dark" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = DARK_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.DarkResistance.Id, KnownSkills.DarkImmunity.Id }
                }
            };
        public static readonly SkillTemplate EarthAbsorption =
            new SkillTemplate(Guid.Parse("937eee0c-a87d-4aba-bd25-9bf1f85fa578"))
            {
                Name = "Absorption: Earth",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "earth" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = EARTH_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.EarthImmunity.Id, KnownSkills.EarthResistance.Id }
                }
            };
        public static readonly SkillTemplate IceAbsorption =
            new SkillTemplate(Guid.Parse("99aaf1d6-6ad8-4689-8e51-9f93862bd029"))
            {
                Name = "Absorption: Ice",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "ice" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = ICE_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.IceImmunity.Id, KnownSkills.IceResistance.Id }
                }
            };
        public static readonly SkillTemplate AirAbsorption =
            new SkillTemplate(Guid.Parse("f9df3744-2b57-4ce1-813a-0bd104bbd9dd"))
            {
                Name = "Absorption: Air",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "air" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = AIR_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.AirImmunity.Id, KnownSkills.AirResistance.Id }
                }
            };
        public static readonly SkillTemplate BoltAbsorption =
            new SkillTemplate(Guid.Parse("f49e1119-2a93-4e88-b453-e048ad81d76b"))
            {
                Name = "Absorption: Bolt",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "bolt" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = BOLT_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.BoltImmunity.Id, KnownSkills.BoltResistance.Id }
                }
            };

        public static readonly SkillTemplate PhysicalAbsorption =
            new SkillTemplate(Guid.Parse("263b8aaa-42b5-4c4a-a24a-e1080a901ca4"))
            {
                Name = "Absorption: Physical",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "physical" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = PHYSICAL_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.PhysicalImmunity.Id, KnownSkills.PhysicalResistance.Id }
                }
            };
        public static readonly SkillTemplate LightAbsorption =
            new SkillTemplate(Guid.Parse("1e6c1533-2764-4eaa-bc8d-7fc7e1d7f4a2"))
            {
                Name = "Absorption: Light",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "light" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = LIGHT_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.LightImmunity.Id, KnownSkills.LightResistance.Id }
                }
            };
        public static readonly SkillTemplate FireAbsorption =
            new SkillTemplate(Guid.Parse("a17a9d4d-db45-473e-8188-04d0196c5f23"))
            {
                Name = "Absorption: Fire",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "fire" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = FIRE_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.FireImmunity.Id, KnownSkills.FireResistance.Id }
                }
            };
        public static readonly SkillTemplate PoisonAbsorption = 
            new SkillTemplate(Guid.Parse("50e08296-7d53-4a64-bb63-f0beba1ad07c"))
            {
                Name = "Absorption: Poison",
                IsSpecialRule = false,
                TargetType = typeof(BeastResistance),
                Keywords = new HashSet<string> { "absorb", "poison" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [AFFINITY_ID] = ABSORBS.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [DAMAGE_TYPE_NAME] = POISON_NAME,
                    OtherKnownSkillsRequired = new[] { KnownSkills.PoisonImmunity.Id, KnownSkills.PoisonResistance.Id }
                }
            };

        public static readonly IDictionary<Guid, SkillTemplate> AbsorptionSkills = new Dictionary<Guid, SkillTemplate>
        {
            { DamageConstants.DARK_DAMAGE_TYPE, DarkAbsorption },
            { DamageConstants.EARTH_DAMAGE_TYPE, EarthAbsorption },
            { DamageConstants.ICE_DAMAGE_TYPE, IceAbsorption },
            { DamageConstants.AIR_DAMAGE_TYPE, AirAbsorption },
            { DamageConstants.BOLT_DAMAGE_TYPE, BoltAbsorption },
            { DamageConstants.PHYSICAL_DAMAGE_TYPE, PhysicalAbsorption },
            { DamageConstants.LIGHT_DAMAGE_TYPE, LightAbsorption },
            { DamageConstants.FIRE_DAMAGE_TYPE, FireAbsorption },
            { DamageConstants.POISON_DAMAGE_TYPE, PoisonAbsorption },
        };

        internal static SkillTemplate GetAbsorptionSkill(Guid damageTypeId)
        {
            return AbsorptionSkills[damageTypeId];
        }

        #endregion

        #region Stats Skills


        public static readonly SkillTemplate SpellCasterMoreMP =
            new SkillTemplate(Guid.Parse("908dbb16-f4ca-4cdc-8eed-a245da2c5a94"))
            {
                Name = "SpellCaster: Boosted MP",            
                IsSpecialRule = false,
                TargetType = typeof(BeastTemplate),
                Keywords = new HashSet<string> { "spell", "cast" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [MP_BOOST] = 10.ToString() ,
                    [NUM_SPELLS] = 1.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate SpellCasterMoreSpells =
            new SkillTemplate(Guid.Parse("3a3a5942-daf3-4073-9bda-52494aee2a7c"))
            {
                Name = "SpellCaster: More Spells",
                IsSpecialRule = false,
                TargetType = typeof(BeastTemplate),
                Keywords = new HashSet<string> { "spell", "cast" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [MP_BOOST] = 0.ToString(),
                    [NUM_SPELLS] = 2.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate ImprovedHitPoints =
            new SkillTemplate(Guid.Parse("82c10f65-10cb-4e61-9a3a-05219d897c85"))
            {
                Name = "Improved Hit Points",
                IsSpecialRule = false,
                TargetType = typeof(BeastTemplate),
                Keywords = new HashSet<string> { "hp" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [HP_BOOST] =  10.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate ImprovedInitiative =
             new SkillTemplate(Guid.Parse("39f25f67-527c-41d6-9c5d-b99a3c9c8b8d"))
             {
                 Name = "Improved Initiative",
                 IsSpecialRule = false,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "init" },
                 OtherAttributes = new SkillAttributeCollection
                {
                    [INIT_BOOST] = 4.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate ImprovedDefensesPhysical =
             new SkillTemplate(Guid.Parse("7f5d781a-f11b-42f5-b41b-932ddb5911df"))
             {
                 Name = "Improved Defense - Physical",
                 IsSpecialRule = false,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "def", "mdef" },
                 OtherAttributes = new SkillAttributeCollection
                {
                    [ DEF_BOOST] =  2.ToString() ,
                    [MDEF_BOOST] = 1.ToString() ,
                    [DEFENSE_SKILL_LIMIT] = DEF_BOOST_LIMIT.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate ImprovedDefensesMagical =
             new SkillTemplate(Guid.Parse("a6959eab-21f3-430a-b199-e646d0a207bf"))
             {
                 Name = "Improved Defense - Magical",
                 IsSpecialRule = false,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "def", "mdef" },
                 OtherAttributes = new SkillAttributeCollection
                {
                    [ DEF_BOOST] = 1.ToString(),
                    [MDEF_BOOST] = 2.ToString(),
                    [DEFENSE_SKILL_LIMIT] = DEF_BOOST_LIMIT.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };
        #endregion

        public static readonly SkillTemplate SpecializedAccuracyCheck =
             new SkillTemplate(Guid.Parse("81b6a98f-00c6-4ff0-ae19-d4b31109020c"))
             {
                 Name = "Specialized: Accuracy Checks",
                 IsSpecialRule = false,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> {  },
                 OtherAttributes = new SkillAttributeCollection
                {
                    [ACC_CHECK] = 3.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                    [SPECIALIZED_SKILL_LIMIT] = SPEC_SKILL_LIMIT.ToString(),
                 }
             };

        public static readonly SkillTemplate SpecializedMagicCheck =
             new SkillTemplate(Guid.Parse("3e5f8736-0fda-422f-8c06-2249cfbdfe5e"))
             {
                 Name = "Specialized: Magic Checks",
                 IsSpecialRule = false,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [ACC_CHECK] = 3.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                     [SPECIALIZED_SKILL_LIMIT] = SPEC_SKILL_LIMIT.ToString(),
                 }
             };

        public static readonly SkillTemplate ConstructSkillPoisonedImmunity =
             new SkillTemplate(Guid.Parse("c5dec586-6386-4631-9120-8ab9b5d83778"))
             {
                 Name = "Construct: Immune to Poisoned",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "poisoned", "construct" },
                 OtherAttributes = new SkillAttributeCollection
                {                    
                    [FREE_SPECIES] = CONSTRUCT.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate ElementalSkillPoisonedImmunity =
             new SkillTemplate(Guid.Parse("8b93d90a-778d-4ef8-b576-baff411b9208"))
             {
                 Name = "Elemental: Immune to Poisoned",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "poisoned", "elemental" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = ELEMENTAL.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };


        public static readonly SkillTemplate PlantSkillDazedImmunity =
             new SkillTemplate(Guid.Parse("0f109d65-09f7-4b22-bdf5-b36551891579"))
             {
                 Name = "Plant: Immune to Dazed",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "dazed", "plant" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = PLANT.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };
        
        public static readonly SkillTemplate PlantSkillShakenImmunity =
             new SkillTemplate(Guid.Parse("c7c8be49-4762-47ed-99be-50d50742c488"))
             {
                 Name = "Plant: Immune to Shaken",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "shaken", "plant" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = PLANT.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate PlantSkillEnragedImmunity =
             new SkillTemplate(Guid.Parse("e8aaa17c-ea36-44b6-a7b4-b207d31ffd56"))
             {
                 Name = "Plant: Immune to Enraged",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "enraged", "plant" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = PLANT.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate UndeadSkillPoisonedImmunity =
             new SkillTemplate(Guid.Parse("aa3c5b8e-9292-4d4c-9a62-12ecf5925519"))
             {
                 Name = "Undead: Immune to Poisoned",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "poisoned", "undead" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = UNDEAD.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };
        public static readonly SkillTemplate UndeadSkillHealingHurts =
             new SkillTemplate(Guid.Parse("084b1a59-659b-4849-b429-b1090d7f1c15"))
             {
                 Name = "Undead: HP Recovery hurts",
                 Text = "Additionally, when an effect (such as an Arcanum, a potion or a spell) would cause an undead creature to recover Hit Points, whoever controls that effect may instead have the undead lose half as many Hit Points.",
                 IsSpecialRule = true,
                 TargetType = typeof(BeastTemplate),
                 Keywords = new HashSet<string> { "immune", "enraged", "undead" },
                 OtherAttributes = new SkillAttributeCollection
                 {
                     [FREE_SPECIES] = UNDEAD.ToString(),
                     [IS_SPECIES_SKILL] = true.ToString(),
                     [IS_KNOWN_SKILL] = true.ToString(),
                 }
             };

        public static readonly SkillTemplate UseEquipment =
            new SkillTemplate(Guid.Parse("32441488-4181-4035-8f4c-84c18ca19ea9"))
            {
                Name = "Use Equipment",
                IsSpecialRule = false,
                Text = "Can equip armor, weapons, and accessories",
                TargetType = typeof(BeastTemplate),
                Keywords = new HashSet<string> { "equipment" },
                OtherAttributes = new SkillAttributeCollection
                {
                    [BLOCKED_SPECIES] = BEAST.ToString(),
                    [FREE_SPECIES] = HUMANOID.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate ImprovedDamageAttack =
            new SkillTemplate(Guid.Parse("560edf3e-4a3a-4167-9f8f-a99235886b45"))
            {

                Name = "Improved Damage: Basic Attack",
                TargetType = typeof(BasicAttackTemplate),
                Text = "boosted attack damage",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { },
                OtherAttributes = new SkillAttributeCollection
                {
                    [DAMAGE_BOOST] = 5.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public static readonly SkillTemplate ImprovedDamageSpell =
            new SkillTemplate(Guid.Parse("91e6bae3-c1a5-4148-86af-f3ad92cb7040"))
            {

                Name = "Improved Damage: Spell",
                TargetType = typeof(SpellTemplate),
                Text = "boosted spell damage",
                IsSpecialRule = false,
                Keywords = new HashSet<string> { },
                OtherAttributes = new SkillAttributeCollection
                {
                    [DAMAGE_BOOST] =  5.ToString(),
                    [IS_KNOWN_SKILL] = true.ToString(),
                }
            };

        public const string IS_KNOWN_SKILL = "IsKnownSkill";

        public static IEnumerable<SkillTemplate> GetAllKnownSkills()
        {
            var allKnownSkills = typeof(KnownSkills)
                .GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(f => f.FieldType == typeof(SkillTemplate))
                .Select(f => f.GetValue(null) as SkillTemplate)
                .ToArray();
            return allKnownSkills;
        }

        internal static SkillTemplate GetKnownSkill(Guid id)
        {
            return GetAllKnownSkills().Single(s => s.Id == id);
        }
    }
}
