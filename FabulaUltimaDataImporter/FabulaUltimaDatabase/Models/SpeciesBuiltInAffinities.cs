namespace FabulaUltimaDatabase.Models
{
    public class SpeciesBuiltInAffinities
    {
        public int NumVulnerabilityChoices { get; set; }
        public required ICollection<Resistance> VulnerabilityChoices { get; set; }
    }
}