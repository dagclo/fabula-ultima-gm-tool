namespace FabulaUltimaDatabase.Models
{
    public struct SkillEntry
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string TargetType { get; internal set; }
        public string Text { get; internal set; }
        public int IsSpecialRule { get; internal set; }
        public string Keywords { get; internal set; }
        public string? OtherAttributes { get; internal set; }
        public int IsAction { get; internal set; }
    }
}