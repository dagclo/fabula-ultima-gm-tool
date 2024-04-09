

using FabulaUltimaDatabase.Models;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using System.Collections;
using static FabulaUltimaSkillLibraryTests.Constants;

namespace FabulaUltimaSkillLibraryTests
{
    public class NpcDataClass
    {
        private static IDictionary<string, Guid> AffToAffinityIdMap =
            new Dictionary<string, Guid>
            {
                { "", DamageConstants.NO_AFFINITY },
                { "VU", DamageConstants.VULNERABLE },
                { "RS", DamageConstants.RESISTANT },
                { "IM", DamageConstants.IMMUNE },
                { "AB", DamageConstants.ABSORBS },
            };

        

        public static IReadOnlyDictionary<string, BeastResistance> GetResistances(
                string physical = "",
                string air = "",
                string bolt = "",
                string dark = "",
                string earth = "",
                string fire = "",
                string ice = "",
                string light = "",
                string poison = ""
            )
        {
            

            return new Dictionary<string, BeastResistance>
                        {
                            { 
                                PHYSICAL.Name, 
                                new BeastResistance
                                { 
                                    Affinity = physical, 
                                    AffinityId = AffToAffinityIdMap[physical], 
                                    DamageType = PHYSICAL.Name,
                                    DamageTypeId = PHYSICAL.Id
                                } 
                            },
                            { 
                                AIR.Name, 
                                new BeastResistance
                                { 
                                    Affinity = air, 
                                    AffinityId = AffToAffinityIdMap[air],
                                    DamageType = AIR.Name,
                                    DamageTypeId = AIR.Id
                                } 
                            },
                            { 
                                BOLT.Name, 
                                new BeastResistance
                                { 
                                    Affinity = bolt, 
                                    DamageType = BOLT.Name,
                                    AffinityId = AffToAffinityIdMap[bolt],
                                    DamageTypeId = BOLT.Id
                                } 
                            },
                            { 
                                DARK.Name, 
                                new BeastResistance
                                { 
                                    Affinity = dark, 
                                    DamageType = DARK.Name,
                                    AffinityId = AffToAffinityIdMap[dark],
                                    DamageTypeId = DARK.Id
                                } 
                            },
                            { 
                                EARTH.Name, 
                                new BeastResistance
                                { 
                                    Affinity = earth, 
                                    DamageType = EARTH.Name,
                                    AffinityId = AffToAffinityIdMap[earth],
                                    DamageTypeId = EARTH.Id
                                } 
                            },
                            { 
                                FIRE.Name, 
                                new BeastResistance
                                { 
                                    Affinity = fire, 
                                    DamageType = FIRE.Name,
                                    AffinityId = AffToAffinityIdMap[fire],
                                    DamageTypeId = FIRE.Id
                                } 
                            },
                            { 
                                ICE.Name, 
                                new BeastResistance
                                { 
                                    Affinity = ice, 
                                    DamageType = ICE.Name,
                                    AffinityId = AffToAffinityIdMap[ice],
                                    DamageTypeId = ICE.Id
                                } 
                            },
                            { 
                                LIGHT.Name, 
                                new BeastResistance
                                { 
                                    Affinity = light, 
                                    DamageType = LIGHT.Name,
                                    AffinityId = AffToAffinityIdMap[light],
                                    DamageTypeId = LIGHT.Id
                                } 
                            },
                            { 
                                POISON.Name, 
                                new BeastResistance
                                { 
                                    Affinity = poison, 
                                    DamageType = POISON.Name,
                                    AffinityId = AffToAffinityIdMap[poison],
                                    DamageTypeId = POISON.Id
                                } 
                            },
                        };
        }

        public static IEnumerable TestCases
        {
            get
            {
                var cutterPillarId = Guid.NewGuid();
                var mandibleSlashId = Guid.NewGuid();
                var cutterBallId = Guid.NewGuid();

                yield return new TestCaseData(
                    null,
                    new BeastTemplate(new BeastModel
                    {
                        Name = "Cutterpillar",
                        Description = "A large centipede that can roll itself into a ball to fend off attacks, only to spring up and bite afterward",
                        Dexterity = D8,
                        Insight = D6,
                        Might = D10,
                        WillPower = D8,
                        Id = cutterPillarId,
                        Level = 5,
                        Species = BEAST,
                        Traits = "heavy, resilient, slow, territorial",
                        Resistances = GetResistances(dark: "RS", earth: "RS", fire: "VU", ice: "VU"),
                        BasicAttacks = new[]
                        {
                            new BasicAttackTemplate
                            {
                                DamageMod = 5,
                                AttackMod = 0,
                                Name = "Mandible Slash",
                                DamageType = POISON,
                                Attribute1 = DEXTERITY,
                                Attribute2 = MIGHT,
                                IsRanged = false,
                                Id = mandibleSlashId,
                            },
                            new BasicAttackTemplate
                            {
                                DamageMod = 5,
                                AttackMod = 0,
                                Name = "Cutter ball",
                                DamageType = PHYSICAL,
                                Attribute1 = DEXTERITY,
                                Attribute2 = MIGHT,
                                IsRanged = false,
                                Id = cutterBallId                                
                            }
                        }
                    }), 
                    new SkillInputData
                    {
                        MaxHP = 60, 
                        MaxMP = 45,
                        DefMod = 2,
                        DefOverride = null,
                        MDefMod = 1,                        
                        
                        Init = 7,
                        AttackModifiers = new Dictionary<Guid, AttackModifier>()
                        {
                            { mandibleSlashId, new AttackModifier { AtkMod =  0, DamMod = 5, Text = "and the target suffers weak", AttackId = mandibleSlashId } },
                            { cutterBallId, new AttackModifier { AtkMod =  0, DamMod = 5, Text = "If the cutterpillar performed the Guard action during its previous turn, this attacks deals 5 extra damage", AttackId = cutterBallId } }
                        }
                    },                    
                    new (SkillTemplate skill, Guid? targetId)? []
                    {
                        ( KnownSkills.SpecialAttackSufferWeak, mandibleSlashId),
                        ( KnownSkills.SpecialAttackOnGuardExtraDamage, cutterBallId),
                        ( KnownSkills.FireVulnerability, null),
                        ( KnownSkills.IceVulnerability, null),
                        ( KnownSkills.DarkResistance, null),
                        ( KnownSkills.EarthResistance, null),
                        ( KnownSkills.ImprovedDefensesPhysical, null),
                        null, // 
                        null,
                    }).SetName("CutterPillar pg 324");

                var whiteMawId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "White Howler",
                                    Description = "White howlers are rarely seen near urban areas since they prefer mountains and forests. They are breathtakingly beautiful.",
                                    Dexterity = D8,
                                    Insight = D8,
                                    Might = D10,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 20,
                                    Species = BEAST,
                                    Traits = ": brave, cunning, regal, vigilant",
                                    Resistances = GetResistances(air: "RS", fire: "VU", ice: "RS"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "White Maw",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = whiteMawId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "IceBerg",                                            
                                        },
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Lick Wounds",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 90,
                                    MaxMP = 60,
                                    DefMod = 2,
                                    DefOverride = null,
                                    MDefMod = 1,
                                    Init = 8,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {   
                                        { whiteMawId, new AttackModifier { AtkMod =  5, DamMod = 15, Text = "and the target suffers weak", AttackId = whiteMawId } },
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackSufferWeak, whiteMawId),
                                    ( KnownSkills.ImprovedDamageAttack, whiteMawId),
                                    ( KnownSkills.FireVulnerability, null),
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.AirResistance, null),                                                    
                                    ( KnownSkills.ImprovedDefensesPhysical, null),
                                    ( KnownSkills.SpellCasterMoreSpells, null),
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    null,
                                }).SetName("White Howler pg 327");

                var elementalDischargeId = Guid.NewGuid();

                var arcaneLanternSkill = new SkillTemplate(Guid.NewGuid())
                {
                    IsSpecialRule = false,
                    Keywords = new HashSet<string>{ "arcane", "lantern", "roll", "damage", "type"},
                    Name = "Elemental Discharge Special Attack",
                    TargetType = typeof(BasicAttackTemplate),
                    Text = "When the arcane lantern performs this attack, roll a d6 to determine the damage type: 1-2 bolt; 3-4 fire; 5-6 ice.",
                    OtherAttributes = new SkillAttributeCollection
                    {
                       [KnownSkills.IS_SPECIAL_ATTACK] = true.ToString()
                    }
                };

                yield return new TestCaseData(
                                new[] { arcaneLanternSkill },
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Arcane Lantern",
                                    Description = "Mages often keep these creatures as magical repositories. In a pinch,\r\nthe lanterns can even help in battle.",
                                    Dexterity = D8,
                                    Insight = D8,
                                    Might = D6,
                                    WillPower = D10,
                                    Id = Guid.NewGuid(),
                                    Level =5,
                                    Species = CONSTRUCT,
                                    Traits = "glowing, helpful, magical, tiny",
                                    Resistances = GetResistances(physical: "VU", fire: "RS", ice: "RS", earth: "RS", poison: "IM"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Elemental Discharge",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = true,
                                            Id = elementalDischargeId,
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 40,
                                    MaxMP = 55,
                                    DefMod = 1,
                                    DefOverride = null,
                                    MDefMod = 2,
                                    Init = 8,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        { elementalDischargeId, new AttackModifier { AttackId = elementalDischargeId, AtkMod =  0, DamMod = 5, Text = "When the arcane lantern performs this attack, roll a d6 to determine the damage type: 1-2 bolt, 2-4, fire, 5-6 ice" } },
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( arcaneLanternSkill, elementalDischargeId),                                    
                                    ( KnownSkills.PhysicalVulnerability, null),
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.FireResistance, null),
                                    ( KnownSkills.EarthResistance, null),
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.ConstructSkillPoisonedImmunity, null),
                                    ( KnownSkills.ImprovedDefensesMagical, null),                                    
                                    null,
                                }).SetName("Arcane Lantern pg 328");

                var razorDiveFreeSkill = new SkillTemplate(Guid.NewGuid())
                {
                    IsSpecialRule = false,
                    Keywords = new HashSet<string> { "Flying", "loses", "benefits", "razorbird" },
                    Name = "Razor Dive Special Attack",
                    TargetType = typeof(BasicAttackTemplate),
                    Text = "After performing this attack, the razorbird loses all benefits granted by the Flying skill until the start of its next turn",
                    OtherAttributes = new SkillAttributeCollection
                    {
                        [KnownSkills.IS_SPECIAL_ATTACK] =  true.ToString(),
                        [KnownSkills.IS_SPECIAL_ATTACK_DETRIMENT] = true.ToString() ,
                    }
                };

                var razorDiveId = Guid.NewGuid();
                var gatlingGunId = Guid.NewGuid();
                var scorchRocketId = Guid.NewGuid();
                yield return new TestCaseData(
                                new[] { razorDiveFreeSkill },
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "RazorBird",
                                    Description = "Often found in the aerial forces of large empires, razorbirds carry a\r\nmighty arsenal of magitech weapons.",
                                    Dexterity = D10,
                                    Insight = D8,
                                    Might = D8,
                                    WillPower = D6,
                                    Id = Guid.NewGuid(),
                                    Level = 15,
                                    Species = CONSTRUCT,
                                    Traits = "fast, flying, heavily armed, loyal",
                                    Resistances = GetResistances(air: "VU", bolt: "VU", ice: "VU", earth: "RS", poison: "IM", fire:"IM"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Razor Dive",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = DEXTERITY,
                                            IsRanged = false,
                                            Id = razorDiveId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Gatling Gun",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = true,
                                            Id = gatlingGunId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Scorch Rocket",
                                            DamageType = FIRE,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = true,
                                            Id = scorchRocketId,
                                        },
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 80,
                                    MaxMP = 45,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        { 
                                            razorDiveId, 
                                            new AttackModifier 
                                            { 
                                                AttackId = razorDiveId, 
                                                AtkMod =  1, 
                                                DamMod = 10, 
                                                Text = "After performing this attack, the razorbird loses all benefits granted by the Flying Skill until the start of its next turn." 
                                            } 
                                        },
                                        {
                                            gatlingGunId,
                                            new AttackModifier
                                            {
                                                AttackId = gatlingGunId,
                                                AtkMod =  1,
                                                DamMod = 5,
                                                Text = "This attack has multi (2)"
                                            }
                                        },
                                        {
                                            scorchRocketId,
                                            new AttackModifier
                                            {
                                                AttackId = scorchRocketId,
                                                AtkMod =  1,
                                                DamMod = 10                                                
                                            }
                                        },
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( razorDiveFreeSkill, razorDiveId),
                                    ( KnownSkills.ImprovedDamageAttack, razorDiveId),
                                    ( KnownSkills.ImprovedDamageAttack, scorchRocketId),
                                    ( KnownSkills.SpecialAttackTwoTargets, gatlingGunId),
                                    ( KnownSkills.AirVulnerability, null),
                                    ( KnownSkills.BoltVulnerability, null),
                                    ( KnownSkills.IceVulnerability, null),
                                    ( KnownSkills.FireImmunity, null),
                                    ( KnownSkills.EarthResistance, null),
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.ConstructSkillPoisonedImmunity, null),                                                                   
                                    ( KnownSkills.ImprovedHitPoints, null),
                                    null,
                                    null,
                                }).SetName("Razorbird pg 330");
                var sharpTurnId = Guid.NewGuid();                
                
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Lightning Wheel",
                                    Description = "Born from the fears of travelers, these cruel demons ride inside a\r\nlarge wooden wheel surrounded by lightning.",
                                    Dexterity = D12,
                                    Insight = D6,
                                    Might = D6,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 10,
                                    Species = DEMON,
                                    Traits = "bright, cackling, cruel, fast",
                                    Resistances = GetResistances( bolt: "AB", dark: "RS", earth: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Sharp Turn",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = sharpTurnId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Fulgur",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 60,
                                    MaxMP = 60,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            sharpTurnId,
                                            new AttackModifier
                                            {
                                                AttackId = sharpTurnId,
                                                AtkMod =  1,
                                                DamMod = 10,                                                
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    
                                    ( KnownSkills.ImprovedDamageAttack, sharpTurnId),
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.BoltResistance, null),
                                    ( KnownSkills.BoltAbsorption, null),
                                    ( KnownSkills.DarkResistance, null),
                                    ( KnownSkills.EarthVulnerability, null),
                                    ( KnownSkills.ImprovedHitPoints, null),                                    
                                    null,
                                }).SetName("Lightning Wheel pg 332");

                var coldGlareId = Guid.NewGuid();
                var viperTangleId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Echidna",
                                    Description = "These dangerous fiends are the embodiments of suspicion and strife.\r\nTheir legs end in a twisting swarm of snakes.",
                                    Dexterity = D8,
                                    Insight = D10,
                                    Might = D6,
                                    WillPower = D10,
                                    Id = Guid.NewGuid(),
                                    Level = 20,
                                    Species = DEMON,
                                    Traits = "clever, knowledgeable, slithering, unfathomable",
                                    Resistances = GetResistances(air: "VU", dark: "RS", fire: "IM", ice: "RS", light: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Tangle of Vipers",
                                            DamageType = POISON,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = false,
                                            Id = viperTangleId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Cold Glare",
                                            DamageType = ICE,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = WILLPOWER,
                                            IsRanged = true,
                                            Id = coldGlareId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Brain Melt",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 70,
                                    MaxMP = 80,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            viperTangleId,
                                            new AttackModifier
                                            {
                                                AttackId = viperTangleId,
                                                AtkMod =  5,
                                                DamMod = 15,
                                            }
                                        },
                                        {
                                            coldGlareId,
                                            new AttackModifier
                                            {
                                                AttackId = coldGlareId,
                                                AtkMod =  5,
                                                DamMod = 10,
                                                Text = "the target cannot perform the Objective action on their next turn. This attack targets Magic Defense."
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {

                                    ( KnownSkills.ImprovedDamageAttack, viperTangleId),                                                                        
                                    ( KnownSkills.SpecialAttackTargetMagicDefense, coldGlareId),
                                    ( KnownSkills.SpecialAttackPreventObjectiveAction, coldGlareId),
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.AirVulnerability, null),                                    
                                    ( KnownSkills.DarkResistance, null),
                                    ( KnownSkills.FireImmunity, null),
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.LightVulnerability, null),
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    null,
                                }).SetName("Echidna pg 333");

                var sharpNeedleId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Acorn Pixie",
                                    Description = "These fairies gather in places where life force flows untainted. If said\r\nenergy becomes corrupted, their minds may grow clouded by rage.",
                                    Dexterity = D10,
                                    Insight = D6,
                                    Might = D6,
                                    WillPower = D10,
                                    Id = Guid.NewGuid(),
                                    Level = 5,
                                    Species = ELEMENTAL,
                                    Traits = "curious, glowing, kind, playful",
                                    Resistances = GetResistances(air: "VU", dark: "VU", earth: "IM", light: "IM", poison: "IM"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Sharp Needle",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = DEXTERITY,
                                            IsRanged = false,
                                            Id = sharpNeedleId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Entangle",
                                        },
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Heal",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 40,
                                    MaxMP = 55,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 8,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            sharpNeedleId,
                                            new AttackModifier
                                            {
                                                AttackId = sharpNeedleId,
                                                AtkMod =  0,
                                                DamMod = 5,
                                                Text = "This attack deals 5 extra damage against slow targets"
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackExtraDamageOnSlow, sharpNeedleId),
                                    ( KnownSkills.SpellCasterMoreSpells, null),
                                    ( KnownSkills.AirVulnerability, null),
                                    ( KnownSkills.DarkVulnerability, null),
                                    ( KnownSkills.EarthImmunity, null),
                                    ( KnownSkills.LightImmunity, null),
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.ElementalSkillPoisonedImmunity, null),
                                    null,
                                }).SetName("Acorn Pixie pg 334");

                var seasonalTouchId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Nymph",
                                    Description = "These spirits dwell within woods, lakes, mountains, and rivers. While\r\ngenerally peaceful, they will fiercely defend their dwellings",
                                    Dexterity = D8,
                                    Insight = D10,
                                    Might = D6,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 15,
                                    Species = ELEMENTAL,
                                    Traits = "fast, territorial, wary, wise",
                                    Resistances = GetResistances(fire: "RS", ice: "RS", earth: "IM", poison: "IM", dark: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Seasonal Touch",
                                            DamageType = AIR,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = false,
                                            Id = seasonalTouchId,
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 70,
                                    MaxMP = 55,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            seasonalTouchId,
                                            new AttackModifier
                                            {
                                                AttackId = seasonalTouchId,
                                                AtkMod =  1,
                                                DamMod = 10,
                                                Text = "Creatures hit by this attack suffer a status effect based on the current season: dazed during spring, shaken during winter, slow during autumn, and weak during summer."
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.FireResistance, null),
                                    ( KnownSkills.EarthImmunity, null),                                    
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.ElementalSkillPoisonedImmunity, null),
                                    ( KnownSkills.ImprovedHitPoints, null),
                                    ( KnownSkills.ImprovedDamageAttack, seasonalTouchId),
                                    ( KnownSkills.DarkVulnerability, null), // added to fix NPC calculation
                                    null,
                                }).SetName("Nymph - undefined special attack - pg 336");

                var heavySpearId = Guid.NewGuid();
                var crossbowId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Guard",                                    
                                    Dexterity = D8,
                                    Insight = D8,
                                    Might = D8,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 5,
                                    Species = HUMANOID,                                    
                                    Resistances = GetResistances(),
                                    Equipment = new[]
                                    {

                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Bronze Plate",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.Parse("0e27bf2b-25e6-4080-8cf7-1c59c50b4d2e"),
                                                Name = "Armor",
                                                IsArmor = true,
                                                IsRanged = false,
                                                IsWeapon = false,
                                            },
                                            StatsModifier = new StatsModifications
                                            {
                                                DefenseModifier = 11,
                                                DefenseOverrides = true,
                                                MagicDefenseModifier = 0,
                                                InitiativeModifier = -3
                                            }
                                        },
                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Heavy Spear",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.Parse("0ecfea40-2859-45f6-bf78-0edd4f8b7d06"),
                                                Name = "Spear",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                            BasicAttack =  new BasicAttackTemplate
                                            {
                                                DamageMod = 12,
                                                AttackMod = 0,
                                                Name = "Heavy Spear",
                                                DamageType = PHYSICAL,
                                                Attribute1 = DEXTERITY,
                                                Attribute2 = MIGHT,
                                                IsRanged = false,
                                                Id = heavySpearId,
                                            }
                                        },
                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Crossbow",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.Parse("0ecfea40-2859-45f6-bf78-0edd4f8b7d06"),
                                                Name = "Spear",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                            BasicAttack =  new BasicAttackTemplate
                                            {
                                                DamageMod = 8,
                                                AttackMod = 0,
                                                Name = "Crossbow",
                                                DamageType = PHYSICAL,
                                                Attribute1 = DEXTERITY,
                                                Attribute2 = INSIGHT,
                                                IsRanged = true,
                                                Id = crossbowId,
                                            }
                                        },
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 60,
                                    MaxMP = 45,
                                    DefMod = 0,
                                    DefOverride = 11,
                                    MDefMod = 0,
                                    Init = 5,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            heavySpearId,
                                            new AttackModifier
                                            {
                                                AttackId = heavySpearId,
                                                AtkMod =  0,
                                                DamMod = 12,                                                
                                            }
                                        },
                                        {
                                            crossbowId,
                                            new AttackModifier
                                            {
                                                AttackId = crossbowId,
                                                AtkMod =  0,
                                                DamMod = 8,
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {                                    
                                    ( KnownSkills.ImprovedHitPoints, null),
                                    ( KnownSkills.UseEquipment, null),
                                    null,
                                    null,
                                }).SetName("Guard pg 338");

                var katanaId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Hivekin",
                                    Dexterity = D10,
                                    Insight = D8,
                                    Might = D8,
                                    WillPower = D6,
                                    Id = Guid.NewGuid(),
                                    Level = 10,
                                    Species = HUMANOID,
                                    Resistances = GetResistances(air: "RS", fire:"VU", poison: "RS", ice: "VU"),
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Bee Dance",
                                        }
                                       
                                    },
                                    Equipment = new[]
                                    {

                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Sage Robe",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.Parse("0e27bf2b-25e6-4080-8cf7-1c59c50b4d2e"),
                                                Name = "Armor",
                                                IsArmor = true,
                                                IsRanged = false,
                                                IsWeapon = false,
                                            },
                                            StatsModifier = new StatsModifications
                                            {
                                                DefenseModifier = 1,
                                                DefenseOverrides = false,
                                                MagicDefenseModifier = 2,
                                                InitiativeModifier = -2
                                            }
                                        },
                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Katana",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.NewGuid(),
                                                Name = "Sword",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                            BasicAttack =  new BasicAttackTemplate
                                            {
                                                DamageMod = 10,
                                                AttackMod = 1,
                                                Name = "Hiveblade",
                                                DamageType = PHYSICAL,
                                                Attribute1 = DEXTERITY,
                                                Attribute2 = INSIGHT,
                                                IsRanged = false,
                                                Id = katanaId,
                                            }
                                        },
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 70,
                                    MaxMP = 50,
                                    DefMod = 1,
                                    DefOverride = null,
                                    MDefMod = 2,
                                    Init = 11,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    { 
                                        {
                                            katanaId,
                                            new AttackModifier
                                            {
                                                AttackId = katanaId,
                                                AtkMod =  5,
                                                DamMod = 10,
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {   
                                    ( KnownSkills.UseEquipment, null),                                    
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    ( KnownSkills.FireVulnerability, null),
                                    ( KnownSkills.AirResistance, null),
                                    ( KnownSkills.PoisonResistance, null),
                                    ( KnownSkills.ImprovedInitiative, null),
                                    ( KnownSkills.IceVulnerability, null),
                                    ( KnownSkills.ImprovedHitPoints, null), 
                                    null,
                                }).SetName("Hivekin adjusted pg 340");

                var scratchId = Guid.NewGuid();
                var ghostFireId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Cait Sith",
                                    Description = "",
                                    Dexterity = D8,
                                    Insight = D8,
                                    Might = D6,
                                    WillPower = D10,
                                    Id = Guid.NewGuid(),
                                    Level = 5,
                                    Species = MONSTER,
                                    Traits = "",
                                    Resistances = GetResistances(bolt: "VU", fire: "RS", ice: "RS", poison: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Scratch",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = scratchId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Ghostfire",
                                            DamageType = FIRE,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = WILLPOWER,
                                            IsRanged = true,
                                            Id = ghostFireId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Heat Control",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 40,
                                    MaxMP = 65,
                                    DefMod = 1,
                                    DefOverride = null,
                                    MDefMod = 2,
                                    Init = 12,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            ghostFireId,
                                            new AttackModifier
                                            {
                                                AttackId = ghostFireId,
                                                AtkMod =  0,
                                                DamMod = 5,
                                                Text = "or ice damage. This attack targets Magic Defense."
                                            }
                                        },
                                                                                {
                                            scratchId,
                                            new AttackModifier
                                            {
                                                AttackId = scratchId,
                                                AtkMod =  0,
                                                DamMod = 5,                                                
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackTargetMagicDefense, ghostFireId),
                                    ( KnownSkills.SpecialAttackAltDamageIce, ghostFireId),
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.BoltVulnerability, null),
                                    ( KnownSkills.FireResistance, null),
                                    ( KnownSkills.IceResistance, null),                                    
                                    ( KnownSkills.PoisonVulnerability, null),
                                    ( KnownSkills.ImprovedDefensesMagical, null),
                                    ( KnownSkills.ImprovedInitiative, null),
                                }).SetName("Cait Sith pg 341");

                var petrifyingPeckId = Guid.NewGuid();
                var toxicPeckId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Cockatrice",
                                    Description = "",
                                    Dexterity = D8,
                                    Insight = D10,
                                    Might = D8,
                                    WillPower = D6,
                                    Id = Guid.NewGuid(),
                                    Level = 15,
                                    Species = MONSTER,
                                    Traits = "",
                                    Resistances = GetResistances(bolt: "RS", earth: "RS", ice: "VU", light: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Petrifying Peck",
                                            DamageType = NO_DAMAGE,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = INSIGHT,
                                            IsRanged = false,
                                            Id = petrifyingPeckId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Toxic Peck",
                                            DamageType = FIRE,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = toxicPeckId,
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 70,
                                    MaxMP = 45,
                                    DefMod = 1,
                                    DefOverride = null,
                                    MDefMod = 2,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            petrifyingPeckId,
                                            new AttackModifier
                                            {
                                                AttackId = petrifyingPeckId,
                                                AtkMod =  4,
                                                DamMod = 0,
                                                Text = "This attack targets Magic Defense instead of Defense. Each target hit by this attack suffers slow; if a target is already slow, they must instead succeed on a DL 10 【MIG + WLP】 Check or be turned to stone — healing a petrified creature is an adventure in and of itself."
                                            }
                                        },
                                                                                {
                                            toxicPeckId,
                                            new AttackModifier
                                            {
                                                AttackId = toxicPeckId,
                                                AtkMod =  4,
                                                DamMod = 10,
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackTargetMagicDefense, petrifyingPeckId),
                                    ( KnownSkills.SpecialAttackSufferSlow, petrifyingPeckId),
                                    ( KnownSkills.ImprovedDamageAttack, toxicPeckId),
                                    ( KnownSkills.BoltResistance, null),
                                    ( KnownSkills.EarthResistance, null),
                                    ( KnownSkills.IceVulnerability, null),          
                                    ( KnownSkills.LightVulnerability, null),
                                    ( KnownSkills.ImprovedDefensesMagical, null),     
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    null,
                                }).SetName("Cockatrice pg 345 - modified");

                var vineSlapId = Guid.NewGuid();
                var alrauneScreamId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Alraune",
                                    Description = "",
                                    Dexterity = D10,
                                    Insight = D8,
                                    Might = D6,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 5,
                                    Species = PLANT,
                                    Traits = "",
                                    Resistances = GetResistances(air: "RS", earth: "RS", ice: "VU", fire: "VU", poison: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Vine Slap",
                                            DamageType = PHYSICAL,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = vineSlapId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Alraune Scream",
                                            DamageType = NO_DAMAGE,
                                            Attribute1 = WILLPOWER,
                                            Attribute2 = WILLPOWER,
                                            IsRanged = true,
                                            Id = alrauneScreamId,
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 50,
                                    MaxMP = 45,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 9,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            vineSlapId,
                                            new AttackModifier
                                            {
                                                AttackId = vineSlapId,
                                                AtkMod =  0,
                                                DamMod = 5,
                                                Text = "This attack deals 5 extra damage to shaken targets."
                                            }
                                        },
                                                                                {
                                            alrauneScreamId,
                                            new AttackModifier
                                            {
                                                AttackId = alrauneScreamId,
                                                AtkMod =  0,
                                                DamMod = 0,
                                                Text = "and the target suffers shaken. This attack targets Magic Defense and has no effect on targets unable to hear the alraune."
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackTargetMagicDefense, alrauneScreamId),
                                    ( KnownSkills.SpecialAttackSufferShaken, alrauneScreamId),
                                    ( KnownSkills.SpecialAttackExtraDamageOnShaken, vineSlapId),
                                    ( KnownSkills.AirResistance, null),
                                    ( KnownSkills.EarthResistance, null),
                                    ( KnownSkills.IceVulnerability, null),
                                    ( KnownSkills.FireVulnerability, null),
                                    ( KnownSkills.PoisonVulnerability, null),
                                    ( KnownSkills.PlantSkillDazedImmunity, null),
                                    ( KnownSkills.PlantSkillShakenImmunity, null),
                                    ( KnownSkills.PlantSkillEnragedImmunity, null),
                                    ( KnownSkills.ImprovedHitPoints, null),
                                }).SetName("Alraune pg 346 - Modified");


                var lashingVinesId = Guid.NewGuid();
                var dragonEaterId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Dragontrap",
                                    Description = "",
                                    Dexterity = D8,
                                    Insight = D8,
                                    Might = D10,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 20,
                                    Species = PLANT,
                                    Traits = "",
                                    Resistances = GetResistances(air: "RS", bolt: "VU", earth:"VU", fire: "RS", light: "RS", poison: "VU"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Dragoneater",
                                            DamageType = PHYSICAL,
                                            Attribute1 = MIGHT,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = lashingVinesId,
                                        },
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Lashing Vines",
                                            DamageType = AIR,
                                            Attribute1 = DEXTERITY,
                                            Attribute2 = MIGHT,
                                            IsRanged = true,
                                            Id = dragonEaterId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Numbing Gas",
                                        },
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Pre-digetsion",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 90,
                                    MaxMP = 60,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 8,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            lashingVinesId,
                                            new AttackModifier
                                            {
                                                AttackId = lashingVinesId,
                                                AtkMod =  5,
                                                DamMod = 15,
                                                Text = " and the target suffers weak."
                                            }
                                        },
                                                                                {
                                            dragonEaterId,
                                            new AttackModifier
                                            {
                                                AttackId = dragonEaterId,
                                                AtkMod =  5,
                                                DamMod = 10,
                                                Text = "If a target hit by this attack is weak, they are swallowed by the dragontrap: a swallowed creature will suffer minor (20) physical damage at the beginning of each of the dragontrap's turns and can perform no actions except for Objective (with the goal of freeing themselves). Freeing a swallowed target is a four-sections Clock; a soldier dragontrap can only have one creature swallowed at the same time, but an elite or champion dragontrap can hold up to two creatures in its maws at a time. If a dragontrap has all mouths occupied and swallows a creature, it must also release one of the creatures it had previously swallowed."
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {                                    
                                    ( KnownSkills.SpecialAttackSufferWeak, lashingVinesId),
                                    ( KnownSkills.ImprovedDamageAttack,  lashingVinesId),
                                    ( KnownSkills.AirResistance, null),
                                    ( KnownSkills.BoltVulnerability, null),
                                    ( KnownSkills.EarthVulnerability, null),
                                    ( KnownSkills.FireResistance, null),
                                    ( KnownSkills.LightResistance, null),
                                    ( KnownSkills.PoisonVulnerability, null),
                                    ( KnownSkills.PlantSkillDazedImmunity, null),
                                    ( KnownSkills.PlantSkillShakenImmunity, null),
                                    ( KnownSkills.PlantSkillEnragedImmunity, null),   
                                    ( KnownSkills.SpellCasterMoreSpells, null),
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    null
                                }).SetName("Dragontrap pg 349");

                var staffId = Guid.NewGuid();
                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Skeletal Mage",
                                    Dexterity = D6,
                                    Insight = D8,
                                    Might = D8,
                                    WillPower = D10,
                                    Id = Guid.NewGuid(),
                                    Level = 10,
                                    Species = UNDEAD,
                                    Resistances = GetResistances(dark: "IM", earth: "VU", fire:"RS", ice: "RS", light:"VU", poison: "IM"),
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Umbra",
                                        }

                                    },
                                    Equipment = new[]
                                    {

                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Sage Robe",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.Parse("0e27bf2b-25e6-4080-8cf7-1c59c50b4d2e"),
                                                Name = "Armor",
                                                IsArmor = true,
                                                IsRanged = false,
                                                IsWeapon = false,
                                            },
                                            StatsModifier = new StatsModifications
                                            {
                                                DefenseModifier = 1,
                                                DefenseOverrides = false,
                                                MagicDefenseModifier = 2,
                                                InitiativeModifier = -2
                                            }
                                        },
                                        new EquipmentTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Staff",
                                            Category = new EquipmentCategory
                                            {
                                                Id = Guid.NewGuid(),
                                                Name = "Arcane",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                            BasicAttack =  new BasicAttackTemplate
                                            {
                                                DamageMod = 6,
                                                AttackMod = 0,
                                                Name = "Staff",
                                                DamageType = PHYSICAL,
                                                Attribute1 = WILLPOWER,
                                                Attribute2 = WILLPOWER,
                                                IsRanged = false,
                                                Id = staffId,
                                            }
                                        },
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 60,
                                    MaxMP = 70,
                                    DefMod = 2,
                                    DefOverride = null,
                                    MDefMod = 4,
                                    Init = 5,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            staffId,
                                            new AttackModifier
                                            {
                                                AttackId = staffId,
                                                AtkMod =  5,
                                                DamMod = 10,
                                                Text = "and the mage recovers 5 MP"
                                            }
                                        }
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.UseEquipment, null),
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.ImprovedDefensesMagical, null),                                    
                                    ( KnownSkills.DarkImmunity, null),
                                    ( KnownSkills.EarthVulnerability, null),
                                    ( KnownSkills.FireResistance, null),
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.LightVulnerability, null),
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.UndeadSkillPoisonedImmunity, null),
                                    ( KnownSkills.UndeadSkillHealingHurts, null)
                                }).SetName("Skeletal Mage pg 351");

                var furyClawId = Guid.NewGuid();

                yield return new TestCaseData(
                                null,
                                new BeastTemplate(new BeastModel
                                {
                                    Name = "Shackled Soul",
                                    Description = "",
                                    Dexterity = D12,
                                    Insight = D8,
                                    Might = D6,
                                    WillPower = D8,
                                    Id = Guid.NewGuid(),
                                    Level = 20,
                                    Species = UNDEAD,
                                    Traits = "",
                                    Resistances = GetResistances(physical: "IM", air:"VU", dark: "IM", earth: "RS", fire: "VU", ice: "RS", light: "VU", poison: "IM"),
                                    BasicAttacks = new[]
                                    {
                                        new BasicAttackTemplate
                                        {
                                            DamageMod = 5,
                                            AttackMod = 0,
                                            Name = "Fury Claw",
                                            DamageType = PHYSICAL,
                                            Attribute1 = MIGHT,
                                            Attribute2 = MIGHT,
                                            IsRanged = false,
                                            Id = furyClawId,
                                        }
                                    },
                                    Spells = new[]
                                    {
                                        new SpellTemplate
                                        {
                                            Id = Guid.NewGuid(),
                                            Name = "Ghastly Wail",
                                        }
                                    }
                                }),
                                new SkillInputData
                                {
                                    MaxHP = 70,
                                    MaxMP = 70,
                                    DefMod = 0,
                                    DefOverride = null,
                                    MDefMod = 0,
                                    Init = 10,
                                    AttackModifiers = new Dictionary<Guid, AttackModifier>()
                                    {
                                        {
                                            furyClawId,
                                            new AttackModifier
                                            {
                                                AttackId = furyClawId,
                                                AtkMod =  5,
                                                DamMod = 10,
                                                Text = "and the target suffers enraged."
                                            }
                                        },
                                    }
                                },
                                new (SkillTemplate skill, Guid? targetId)?[]
                                {
                                    ( KnownSkills.SpecialAttackSufferEnraged, furyClawId),                                    
                                    ( KnownSkills.PhysicalImmunity, null),
                                    ( KnownSkills.AirVulnerability, null),
                                    ( KnownSkills.DarkImmunity, null),
                                    ( KnownSkills.EarthResistance, null),
                                    ( KnownSkills.FireVulnerability, null),
                                    ( KnownSkills.IceResistance, null),
                                    ( KnownSkills.LightVulnerability, null),
                                    ( KnownSkills.PoisonImmunity, null),
                                    ( KnownSkills.SpellCasterMoreMP, null),
                                    ( KnownSkills.SpecializedAccuracyCheck, null),
                                    ( KnownSkills.UndeadSkillPoisonedImmunity, null),
                                    ( KnownSkills.UndeadSkillHealingHurts, null),
                                    null
                                }).SetName("Shackled Soul pg 353");

                var mercyBladeId = Guid.NewGuid();
                var justiceClubId = Guid.NewGuid();
                var bumRushID = Guid.NewGuid();

                yield return new TestCaseData(
                    null,
                    new BeastTemplate(new BeastModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Hooded Assassin",
                        Level = 30,
                        Species = HUMANOID,
                        Dexterity = D10,
                        Insight = D6,
                        Might = D12,
                        WillPower = D6,
                        Resistances = GetResistances(bolt: "VU", dark: "RS", light:"VU", poison: "RS"),
                        Equipment = new[]
                        {
                            new EquipmentTemplate
                            {
                                Id = Guid.NewGuid(),
                                Category = new EquipmentCategory
                                            {
                                                Id = Guid.NewGuid(),
                                                Name = "Dagger",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                BasicAttack = new BasicAttackTemplate
                                {
                                    Id = mercyBladeId,
                                    Name = "Mercy's Blade",
                                    DamageMod = 0,
                                    DamageType = PHYSICAL,
                                    AttackMod = 0,
                                    Attribute1 = DEXTERITY, 
                                    Attribute2 = DEXTERITY,
                                    IsRanged = false
                                }
                            },
                            new EquipmentTemplate
                            {
                                Id = Guid.NewGuid(),
                                Category = new EquipmentCategory
                                            {
                                                Id = Guid.NewGuid(),
                                                Name = "Heavy",
                                                IsArmor = false,
                                                IsRanged = false,
                                                IsWeapon = true,
                                            },
                                BasicAttack = new BasicAttackTemplate
                                {
                                    Id = justiceClubId,
                                    Name = "Justice's Club",
                                    DamageMod = 0,
                                    DamageType = PHYSICAL,
                                    AttackMod = 0,
                                    Attribute1 = MIGHT,
                                    Attribute2 = MIGHT,
                                    IsRanged = false
                                }
                            },
                            new EquipmentTemplate
                            {
                                Id = Guid.NewGuid(),
                                Name = "Executioner's Leather ",
                                Category = new EquipmentCategory
                                {
                                    Id = Guid.Parse("0e27bf2b-25e6-4080-8cf7-1c59c50b4d2e"),
                                    Name = "Armor",
                                    IsArmor = true,
                                    IsRanged = false,
                                    IsWeapon = false,
                                },
                                StatsModifier = new StatsModifications
                                {
                                    DefenseModifier = 1,
                                    DefenseOverrides = false,
                                    MagicDefenseModifier = 1,
                                    InitiativeModifier = -1
                                }
                            },
                        },
                        BasicAttacks = new BasicAttackTemplate[]
                        {
                            new BasicAttackTemplate
                            {
                                Id = bumRushID,
                                DamageMod = 5,
                                AttackMod = 0,
                                Name = "Bum Rush",
                                DamageType = PHYSICAL,
                                Attribute1 = DEXTERITY,
                                Attribute2 = MIGHT,
                                IsRanged = false,
                            }
                        }
                    }),
                    new SkillInputData
                    {
                        MaxHP = 130,
                        MaxMP = 60,
                        DefMod = 1,
                        DefOverride = null,
                        MDefMod = 1,
                        Init = 8,
                        AttackModifiers = new Dictionary<Guid, AttackModifier>()
                        {
                            {
                                mercyBladeId,
                                new AttackModifier
                                {
                                    AttackId = mercyBladeId,
                                    AtkMod =  6,
                                    DamMod = 10,
                                    Text = "target suffers poisoned"
                                }
                            },
                            {
                                justiceClubId,
                                new AttackModifier
                                {
                                    AttackId = justiceClubId,
                                    AtkMod =  6,
                                    DamMod = 10,
                                    Text = "target suffers dazed"
                                }
                            },
                            {
                                bumRushID,
                                new AttackModifier
                                {
                                    AttackId = bumRushID,
                                    AtkMod =  6,
                                    DamMod = 10,
                                    Text = "On success, Hooded assassin has +5 damage on next attack on target"
                                }
                            },
                        }
                    },
                    new (SkillTemplate skill, Guid? targetId)?[]
                    {
                        (KnownSkills.SpecialAttackSufferPoisoned, mercyBladeId),
                        (KnownSkills.ImprovedDamageAttack, mercyBladeId),
                        (KnownSkills.SpecialAttackSufferDazed, justiceClubId),
                        (KnownSkills.ImprovedDamageAttack, justiceClubId),
                        (KnownSkills.BoltVulnerability, null),
                        (KnownSkills.LightVulnerability, null),
                        (KnownSkills.DarkResistance, null),
                        (KnownSkills.PoisonResistance, null),
                        (KnownSkills.UseEquipment, null),
                        (KnownSkills.SpecializedAccuracyCheck, null),
                        (KnownSkills.ImprovedHitPoints, null),
                        null
                    }

                    ).SetName("Hooded Assassin - High Level Original");

                var piercingHugId = Guid.NewGuid();
                var thornBarrageId = Guid.NewGuid();
                

                yield return new TestCaseData(
                    null,
                    new BeastTemplate(new BeastModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Cactroll",
                        Level = 15,
                        Species = PLANT,
                        Dexterity = D8,
                        Insight = D6,
                        Might = D12,
                        WillPower = D6,
                        Resistances = GetResistances(air: "RS", earth: "RS", light: "RS", fire: "RS", ice:"VU"),
                       // no equipment
                        BasicAttacks = new BasicAttackTemplate[]
                        {
                            new BasicAttackTemplate
                            {
                                Id = piercingHugId,
                                DamageMod = 5,
                                AttackMod = 0,
                                Name = "Piercing Hug",
                                DamageType = PHYSICAL,
                                Attribute1 = MIGHT,
                                Attribute2 = MIGHT,
                                IsRanged = false,
                            },
                            new BasicAttackTemplate
                            {
                                Id = thornBarrageId,
                                DamageMod = 5,
                                AttackMod = 0,
                                Name = "Thorn Barrage",
                                DamageType = PHYSICAL,
                                Attribute1 = DEXTERITY,
                                Attribute2 = MIGHT,
                                IsRanged = true,
                            },
                        },
                        Spells = new SpellTemplate[]
                        {
                            new SpellTemplate
                            {
                                Id = Guid.NewGuid(),
                                Name = "Moisture Drain"
                            }
                        }
                    }),
                    new SkillInputData
                    {
                        MaxHP = 90,
                        MaxMP = 55,
                        DefMod = 0,
                        DefOverride = null,
                        MDefMod = 0,
                        Init = 7,
                        AttackModifiers = new Dictionary<Guid, AttackModifier>()
                        {
                            {
                                piercingHugId,
                                new AttackModifier
                                {
                                    AttackId = piercingHugId,
                                    AtkMod =  1,
                                    DamMod = 10,
                                    Text = ""
                                }
                            },
                            {
                                thornBarrageId,
                                new AttackModifier
                                {
                                    AttackId = thornBarrageId,
                                    AtkMod =  1,
                                    DamMod = 5,
                                    Text = ""
                                }
                            }
                        }
                    },
                    new (SkillTemplate skill, Guid? targetId)?[]
                    {   
                        (KnownSkills.ImprovedDamageAttack, piercingHugId),                        
                        (KnownSkills.AirResistance, null),
                        (KnownSkills.EarthResistance, null),
                        (KnownSkills.FireResistance, null),
                        (KnownSkills.LightResistance, null),
                        (KnownSkills.IceVulnerability, null),
                        ( KnownSkills.PlantSkillDazedImmunity, null),
                        ( KnownSkills.PlantSkillShakenImmunity, null),
                        ( KnownSkills.PlantSkillEnragedImmunity, null),
                        ( KnownSkills.SpellCasterMoreMP, null),
                    }

                    ).SetName("Cactroll pg 347");
            }
        }
    }
}