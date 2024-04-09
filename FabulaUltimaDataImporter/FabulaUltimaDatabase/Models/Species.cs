
namespace FabulaUltimaDatabase.Models
{
    public class Species
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
