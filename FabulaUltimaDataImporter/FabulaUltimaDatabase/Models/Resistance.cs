namespace FabulaUltimaDatabase.Models
{
    public class Resistance
    {   
        public Guid BeastTemplateId { get; internal set; }
        public Guid DamageTypeId { get; set; }
        public Guid AffinityId { get; }

        public Resistance() { }

        public Resistance(DamageTypeEntry d, Affinity affinity)
        {
            DamageTypeId = d.Id;
            AffinityId = affinity.Id;            
        }

        public Resistance(Guid damageTypeId, Guid affinityId, Guid beastTemplateId)
        {
            DamageTypeId = damageTypeId; 
            AffinityId = affinityId;
            BeastTemplateId = beastTemplateId;
        }
    }
}