namespace FabulaUltimaSkillLibrary.Models
{
    public class AttackModifier
    {
        public Guid AttackId { get; set; }
        public int AtkMod { get; set; }
        public int DamMod { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
