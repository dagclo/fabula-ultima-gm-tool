namespace FabulaUltimaNpc
{
    public class BeastResistance
    {
        public string DamageType { get; set; }
        public string Affinity { get; set; }
        public Guid AffinityId { get; set; }
        public Guid DamageTypeId { get; set; }

        public override string ToString()
        {
            return $"{nameof(BeastResistance)} - {DamageType}:{Affinity}";
        }
    }
}