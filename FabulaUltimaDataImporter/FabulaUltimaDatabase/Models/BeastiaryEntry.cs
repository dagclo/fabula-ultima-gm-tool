namespace FabulaUltimaDatabase.Models
{
    public class BeastiaryEntry
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public uint Level { get; set; }
        public string? Traits { get; set; }
        public Guid Species { get; set; }
        public uint Insight { get; set; }
        public uint Dexterity { get; set; }
        public uint Might { get; set; }
        public uint WillPower { get; set; }        
        public string? ImageFile { get; set; }
    }
}
