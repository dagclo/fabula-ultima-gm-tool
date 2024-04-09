namespace FabulaUltimaSkillLibrary.Models
{
    public class SkillInputData
    {
        public Guid BeastId { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int DefMod { get; set; }
        public int? DefOverride { get; set; }
        public int MDefMod { get; set; }        
        public IDictionary<Guid, AttackModifier> AttackModifiers { get; set; } = new Dictionary<Guid, AttackModifier>();
        public int Init { get; set; }
        
    }
}
