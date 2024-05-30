using FabulaUltimaDatabase;
using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibrary
{
    public class SpecialAttackIndex
    {
        private readonly Instance _dbInstance;
        private readonly IDictionary<string, ICollection<SkillTemplate>> _keywordToSkillMap;

        public SpecialAttackIndex(Instance db) : this(new SkillTemplate[0], db)
        {
        }

        public SpecialAttackIndex(IEnumerable<SkillTemplate> specialAttacks, Instance dbInstance)
        {
            _dbInstance = dbInstance;
            IEnumerable<SkillTemplate> dbSkills = _dbInstance.GetSkills(new Dictionary<string, string> { { KnownSkills.IS_SPECIAL_ATTACK, true.ToString() } });
            _keywordToSkillMap = new Dictionary<string, ICollection<SkillTemplate>>();
            BuildKeywordToSkillMap(specialAttacks, dbSkills);
        }

        private void BuildKeywordToSkillMap(IEnumerable<SkillTemplate> specialAttacks, IEnumerable<SkillTemplate> dbSkills)
        {
            _keywordToSkillMap.Clear();
            foreach (var attack in specialAttacks.Concat(dbSkills))
            {
                foreach (var keyword in attack.Keywords)
                {
                    var normalizedKeyword = keyword.ToLowerInvariant();
                    if (!_keywordToSkillMap.ContainsKey(normalizedKeyword))
                    {
                        _keywordToSkillMap[normalizedKeyword] = new List<SkillTemplate>();
                    }

                    _keywordToSkillMap[normalizedKeyword].Add(attack);
                }
            }
        }

        internal IEnumerable<SkillTemplate> GetSpecialAttacks(string[] tokens)
        {
            var skillTemplateMap = new Dictionary<Guid, SkillTemplate>();
            var idToKeywordMatchMap = new Dictionary<Guid, ISet<string>>();
            foreach(var token in tokens.Select(t => t.ToLowerInvariant()))
            {
                if (!_keywordToSkillMap.TryGetValue(token, out var templates)) continue;

                foreach(var template in templates)
                {
                    skillTemplateMap[template.Id] = template;
                    if(!idToKeywordMatchMap.ContainsKey(template.Id))
                    {
                        idToKeywordMatchMap[template.Id] = new HashSet<string>();
                    }

                    idToKeywordMatchMap[template.Id].Add(token);
                }
            }

            var skills = idToKeywordMatchMap.Keys
                            .Select(k => skillTemplateMap[k])
                            .Where(s =>
                            {                                
                                var overlap = s.Keywords.Select(k => k.ToLowerInvariant()).Intersect(idToKeywordMatchMap[s.Id]);
                                return overlap.Count() == s.Keywords.Count();
                            })
                            .ToList();

            return skills;
        }

        internal IEnumerable<SkillTemplate?> GetSpecialAttacks(string text)
        {
            return GetSpecialAttacks(text.Split(new[] { " ", ".", ",", ":" }, StringSplitOptions.RemoveEmptyEntries));
        }

        internal void Rebuild()
        {
            IEnumerable<SkillTemplate> dbSkills = _dbInstance.GetSkills(new Dictionary<string, string> { { KnownSkills.IS_SPECIAL_ATTACK, true.ToString() } });            
            BuildKeywordToSkillMap(new SkillTemplate[0], dbSkills);
        }
    }
}