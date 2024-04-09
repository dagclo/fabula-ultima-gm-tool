using FabulaUltimaNpc;

namespace FabulaUltimaSkillLibrary.Models
{
    public class SkillOutputData
    {   
        public ICollection<(SkillTemplate skill, Guid? targetId)?> SkillSlots { get; set; }
        public int RemainingSkillPoints => SkillSlots.Where(s => s == null).Count();
    }
}
