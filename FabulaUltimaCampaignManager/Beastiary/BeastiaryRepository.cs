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
        public Instance Database { get; }

        public BeastiaryRepository(Instance instance)
        {     
            Database = instance;
            
            _damageTypes = new Lazy<IEnumerable<FabulaUltimaDatabase.Models.DamageTypeEntry>>(() => Database.GetDamageTypes().OrderBy(t => t.Name).ToArray());
            _skills = new Lazy<IEnumerable<SkillTemplate>>(() => Database.GetSkills());
        }

        public IEnumerable<IBeastTemplate> GetBeasts()
        {
            return Database.GetBeasts().Select(b => new SkilledBeastTemplateWrapper(b)).OrderBy(sb => sb.Name);
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
            Database.UpdateBeast(template);
        }

        internal void DeleteBeastTemplate(Guid id)
        {
            Database.RemoveBeast(id);
        }
    }
}
