namespace FabulaUltimaNpc
{
    public class ActionTemplate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Effect { get; set; }

        public ActionTemplate Clone()
        {
            return new ActionTemplate
            {
                Id = Id,
                Name = Name,
                Effect = Effect
            };
        }
    }
}
