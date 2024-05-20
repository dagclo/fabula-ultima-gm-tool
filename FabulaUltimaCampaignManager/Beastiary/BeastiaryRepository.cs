using FabulaUltimaDatabase;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FabulaUltimaSkillLibrary;

namespace FirstProject.Beastiary
{
    public class BeastiaryRepository
    {
        private readonly Instance _database;        

        public BeastiaryRepository(Instance instance)
        {     
            _database = instance;
            
            _damageTypes = new Lazy<IEnumerable<FabulaUltimaDatabase.Models.DamageTypeEntry>>(() => _database.GetDamageTypes().OrderBy(t => t.Name).ToArray());
            _skills = new Lazy<IEnumerable<SkillTemplate>>(() => _database.GetSkills());
        }

        public IEnumerable<IBeastTemplate> GetBeasts()
        {
            return _database.GetBeasts().Select(b => new SkilledBeastTemplateWrapper(b)).OrderBy(sb => sb.Name);
        }

        private Lazy<IEnumerable<FabulaUltimaDatabase.Models.DamageTypeEntry>> _damageTypes;
        private Lazy<IEnumerable<SkillTemplate>> _skills;

        public IEnumerable<DamageTypeValue> GetDamageTypes()
        {
            return _damageTypes.Value.Select(t => new DamageTypeValue { Name = t.Name });
        }

        internal IEnumerable<SkillTemplate> GetSkills(bool filterVulnerabilities = true, bool filterSpeciesSkills = true)
        {
            var result = _skills.Value;

            if(filterVulnerabilities == true)
            {
                result = result.Where(s => !s.IsVulnerabilitySkill());
            }

            if(filterSpeciesSkills == true)
            {
                result = result.Where(s => !s.IsSpeciesSkill());
            }

            return result;
        }

        internal void UpdateBeastTemplate(IBeastTemplate template)
        {
            _database.UpdateBeast(template);
        }
    }
}
