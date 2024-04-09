namespace FabulaUltimaNpc
{

    public class SkillTemplate
    {
        public SkillTemplate(Guid id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public Type TargetType { get; set; }
        public string Text { get; set; }
        public bool IsSpecialRule { get; set; }
        public ISet<string> Keywords { get; set; }
        public SkillAttributeCollection OtherAttributes { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{Id} : {Name}";
        }
    }
}
