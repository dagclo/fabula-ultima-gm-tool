using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using System.Text;

namespace FabulaUltimaSkillLibraryTests
{
    internal class KnownSkillTests
    {
        [TestCase]
        public void AllKnownSkillsAreMarked()
        {
            var knownSkills = KnownSkills.GetAllKnownSkills().ToArray();
            Assert.IsTrue(knownSkills.Any());
            bool IsKnownSkill(SkillTemplate skill)
            {
                var isKnownSkillString = skill.OtherAttributes?[KnownSkills.IS_KNOWN_SKILL];
                if(bool.TryParse(isKnownSkillString, out var isKnownSkill)) 
                {
                    return isKnownSkill;
                }
                return false;
            }

            string GetUnmarkedSkills(IEnumerable<SkillTemplate> skills)
            {
                return string.Join(',', skills.Where(s => !IsKnownSkill(s)));
            }
            Assert.IsTrue(knownSkills.All(s => IsKnownSkill(s)), GetUnmarkedSkills(knownSkills));
        }

        [TestCase]
        public void AllKnownSkillsHaveTargetTypes()
        {
            var knownSkills = KnownSkills.GetAllKnownSkills().ToArray();
            Assert.IsTrue(knownSkills.Any());
          

            string GetSkillsWithoutTargetTypes(IEnumerable<SkillTemplate> skills)
            {
                return string.Join(',', skills.Where(s => s.TargetType == null));
            }
            Assert.IsTrue(knownSkills.All(s => s.TargetType != null), GetSkillsWithoutTargetTypes(knownSkills));
        }

        [TestCase]
        public void AllKnownSkillsHaveUniqueIds()
        {
            var knownSkills = KnownSkills.GetAllKnownSkills().ToArray();
            var uniqueSkillIds = knownSkills.Select(s => s.Id).ToHashSet();

            Assert.IsTrue(knownSkills.Any());

            string GetDupeIdsSkills(IEnumerable<SkillTemplate> skills)
            {
                var skillIdGroupsWithDupes = skills.GroupBy(s => s.Id).Where(g => g.Count() > 1);
                var strBlder = new StringBuilder();
                strBlder.AppendLine("dupe ids");
                foreach(var group in skillIdGroupsWithDupes) 
                { 
                    strBlder.Append($"id: {group.Key} ");
                    strBlder.Append(string.Join(',', group));
                    strBlder.Append(Environment.NewLine);
                }
                return strBlder.ToString();
            }

            Assert.That(knownSkills.Count(), Is.EqualTo(uniqueSkillIds.Count()), GetDupeIdsSkills(knownSkills));
        }
    }
}
