namespace FabulaUltimaDatabase.Models
{
    public class SpeciesBuiltInAffinities
    {
        public int NumVulnerabilityChoices { get; set; }
        public ICollection<Resistance> VulnerabilityChoices { get; set; }
    }
}