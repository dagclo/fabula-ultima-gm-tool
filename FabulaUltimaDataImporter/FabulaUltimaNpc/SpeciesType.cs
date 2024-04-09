namespace FabulaUltimaNpc
{
    public class SpeciesType
    {
        public SpeciesType(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public SpeciesType() : this(Guid.Empty, "") { }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
