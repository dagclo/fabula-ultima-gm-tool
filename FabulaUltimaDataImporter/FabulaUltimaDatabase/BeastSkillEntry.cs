namespace FabulaUltimaDatabase
{
    public struct BeastSkillEntry
    {
        public Guid BeastTemplateId { get; set; }
        public Guid SkillId { get; set; }
        public Guid? BasicAttackId { get; set; }
    }
}