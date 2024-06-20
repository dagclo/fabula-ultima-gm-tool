

using FabulaUltimaDatabase.Models;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using NSubstitute;
using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Configuration;
using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibraryTests
{
    public class ResolverTests
    {

        [TestCaseSource(typeof(NpcDataClass), nameof(NpcDataClass.TestCases))]
        public void Resolve_AttachSkillsAndAssignPoints(
            ICollection<SkillTemplate> skillTemplates,
            IBeastTemplate npc, 
            SkillInputData inputData,           
            ICollection<(SkillTemplate skill, Guid? targetId)?> expectedSkills)
        {
            // Arrange
            Instance dbInstanceMock = GenerateInstance(skillTemplates);
            var knownSpecialAttacks = KnownSkills.GetAllKnownSkills().Where(s => s.OtherAttributes.IsSpecialAttack == true);
            var specialAttackIndex = new SpecialAttackIndex(knownSpecialAttacks, dbInstanceMock);
            var resolver = new Resolver(dbInstanceMock, specialAttackIndex);

            // Act
            var skillResolution = resolver.ResolveSkills(npc, inputData);

            // Assert
            Assert.That(skillResolution, Is.Not.Null);
            Assert.Multiple(() =>
            {
                
                var expectedSkillMap = expectedSkills
                    .Where(s => s != null)
                    .GroupBy(s => s.Value.skill.Id)
                    .ToDictionary(g => g.Key, g => g as ICollection<(SkillTemplate skill, Guid? targetId)?>);

                var actualSkillMap = skillResolution.SkillSlots
                    .Where(s => s != null)
                    .GroupBy(s => s.Value.skill.Id)
                    .ToDictionary(g => g.Key, g => g as ICollection<(SkillTemplate skill, Guid? targetId)?>);

                Assert.That(
                    skillResolution.SkillSlots.Count(s => s == null), 
                    Is.EqualTo(expectedSkills.Count(s => s == null)), "skills slots don't match");

                Assert.That(actualSkillMap.Count(), Is.EqualTo(expectedSkillMap.Count()), "skill numbers should match");

                foreach(var pair in expectedSkillMap)
                {
                    Assert.IsTrue(actualSkillMap.ContainsKey(pair.Key), $"skills with id {pair.Key}:{pair.Value?.First()?.skill.Name} missing");
                    if (!actualSkillMap.ContainsKey(pair.Key)) continue;
                    var expectedGroup = pair.Value;
                    var actualGroup = actualSkillMap[pair.Key];
                    Assert.That(actualGroup.Count(), Is.EqualTo(expectedGroup.Count()), $"num of skill {pair.Key} should match");
                    
                    foreach(var entry in expectedGroup)
                    {
                        Assert.IsTrue(actualGroup.Any(
                            s => s.Value.skill.Id == entry.Value.skill.Id && s.Value.targetId == entry.Value.targetId), 
                            $"skill {entry.Value.skill} with target id {entry.Value.targetId} is missing");
                    }                    
                }
            });
        }

        private static IDictionary<Guid, int> SpeciesSkillPointMap = new Dictionary<Guid, int>
                {
                    { Guid.Parse("b0788720-8fa0-4968-ac61-5f3063d97c17"), 4 }, // Beast
                    { Guid.Parse("f50815fc-9d41-4eeb-9797-182544244f0a"), 2 }, // Construct
                    { Guid.Parse("37e76b06-fd97-4c73-8509-eb42e3610eef"), 3 }, // Demon
                    { Guid.Parse("19014999-30a7-4635-b1a1-505b10a5bc19"), 2 }, // Elemental
                    { Guid.Parse("69711547-14c6-4a01-af94-f5d5117a6bae"), 3 }, // Humanoid
                    { Guid.Parse("23e74a9c-8413-497f-b098-f541b43884c0"), 4 }, // Monster
                    { Guid.Parse("d608585c-32ff-4d10-88b9-b4df66364195"), 3 }, // Plant
                    { Guid.Parse("3e35bbec-d713-4efc-af8a-3d5e01403885"), 1 }, // Undead - extra skill point granted by vulnerability
                };

        private static IDictionary<Guid, int> SkillBonusMap = new Dictionary<Guid, int>
                {
                    { Guid.Parse("ffad483e-0ad5-4a43-b235-080ddfd67470"), 2 }, //physical
                    { Guid.Parse("01a0f627-748c-49eb-999f-03746b673be5"), 1 }, // ice
                    { Guid.Parse("dda401cf-437c-438e-9a1b-e3421f9c4902"), 1 }, // fire
                    { Guid.Parse("39fb0c13-06df-47e0-ae4b-ccfb2012b03d"), 1 }, // bolt
                    { Guid.Parse("7dfe93bd-67d5-468d-bb32-b4d8c1676305"), 1 }, // air
                    { Guid.Parse("813f85c6-fa28-42f2-ad4b-b682a4814382"), 1 }, // earth
                    { Guid.Parse("9ef9cb1e-96da-4acc-ae0e-66e8e5236888"), 1 }, // light
                    { Guid.Parse("c635cee1-fcbc-44cd-98c9-fe55c7084806"), 1 }, // dark
                    { Guid.Parse("f36c11bf-a896-4cc7-9460-37bf5100e14a"), 1 }, // poison
                };

        private static IDictionary<Guid, int> SpeciesFreeResistanceMap = new Dictionary<Guid, int>
                {
                    { Guid.Parse("b0788720-8fa0-4968-ac61-5f3063d97c17"), 0 }, // Beast
                    { Guid.Parse("f50815fc-9d41-4eeb-9797-182544244f0a"), 0 }, // Construct
                    { Guid.Parse("37e76b06-fd97-4c73-8509-eb42e3610eef"), 2 }, // Demon
                    { Guid.Parse("19014999-30a7-4635-b1a1-505b10a5bc19"), 0 }, // Elemental
                    { Guid.Parse("69711547-14c6-4a01-af94-f5d5117a6bae"), 0 }, // Humanoid
                    { Guid.Parse("23e74a9c-8413-497f-b098-f541b43884c0"), 0 }, // Monster
                    { Guid.Parse("d608585c-32ff-4d10-88b9-b4df66364195"), 0 }, // Plant
                    { Guid.Parse("3e35bbec-d713-4efc-af8a-3d5e01403885"), 0 }, // Undead
                };

        private static IDictionary<Guid, int> SpeciesFreeImmunityMap = new Dictionary<Guid, int>
                {
                    { Guid.Parse("b0788720-8fa0-4968-ac61-5f3063d97c17"), 0 }, // Beast
                    { Guid.Parse("f50815fc-9d41-4eeb-9797-182544244f0a"), 0 }, // Construct
                    { Guid.Parse("37e76b06-fd97-4c73-8509-eb42e3610eef"), 0 }, // Demon
                    { Guid.Parse("19014999-30a7-4635-b1a1-505b10a5bc19"), 1 }, // Elemental
                    { Guid.Parse("69711547-14c6-4a01-af94-f5d5117a6bae"), 0 }, // Humanoid
                    { Guid.Parse("23e74a9c-8413-497f-b098-f541b43884c0"), 0 }, // Monster
                    { Guid.Parse("d608585c-32ff-4d10-88b9-b4df66364195"), 0 }, // Plant
                    { Guid.Parse("3e35bbec-d713-4efc-af8a-3d5e01403885"), 0 }, // Undead
                };

        private static Instance GenerateInstance(IEnumerable<SkillTemplate> skillTemplates)
        {
            var instanceMock = Substitute.For<Instance>(new DatabaseConfiguration());
            instanceMock.GetNumSkills(Arg.Any<SpeciesType>()).Returns(x =>
            {
                return SpeciesSkillPointMap[x.Arg<SpeciesType>().Id];
            });

            instanceMock.GetSkillBonus(Arg.Any<Guid>()).Returns(x =>
            {
                var damageTypeId = x.Arg<Guid>();
                return SkillBonusMap[damageTypeId];
            });

            var skills = skillTemplates ?? new List<SkillTemplate>();
            instanceMock.GetSkills(Arg.Any<IDictionary<string, string>>()).Returns(x =>
            {
                var filters = x.Arg<IDictionary<string, string>>() ?? new Dictionary<string,string>();
                return skills.
                        Where(s =>
                        {
                            var otherAttr = s.OtherAttributes;
                            if (filters.Any() && !otherAttr.Any()) return false;
                            foreach(var pair in filters)
                            {
                                if (!otherAttr.ContainsKey(pair.Key)) return false;
                                if (otherAttr[pair.Key] != pair.Value) return false;
                            }
                            return true;
                        }).ToArray();
            });

            instanceMock.GetNumFreeResistances(Arg.Any<SpeciesType>())
                .Returns(x =>
                {
                    return SpeciesFreeResistanceMap[x.Arg<SpeciesType>().Id];
                });

            instanceMock.GetNumFreeImmunities(Arg.Any<SpeciesType>())
                .Returns(x =>
                {
                    return SpeciesFreeImmunityMap[x.Arg<SpeciesType>().Id];
                });

            instanceMock.GetBuiltInVulnerbilityChoices(Arg.Any<SpeciesType>())
                .Returns(x =>
                {
                    var speciesType = x.Arg<SpeciesType>();
                    if(speciesType.Id == Constants.PLANT.Id)
                    {
                        return new SpeciesBuiltInAffinities
                        { 
                            NumVulnerabilityChoices = 1,
                            VulnerabilityChoices = new[]
                            {
                                new Resistance()
                                {
                                    DamageTypeId = Constants.AIR.Id,
                                },
                                new Resistance()
                                {
                                    DamageTypeId = Constants.BOLT.Id,
                                },
                                new Resistance()
                                {
                                    DamageTypeId = Constants.FIRE.Id,
                                },
                                 new Resistance()
                                {
                                    DamageTypeId = Constants.ICE.Id,
                                },
                            }
                        };
                    }

                    return new SpeciesBuiltInAffinities
                    {
                        NumVulnerabilityChoices = 0,
                        VulnerabilityChoices = new Resistance[0]
                    };
                });

            return instanceMock;
        }
    }
}