namespace FabulaUltimaNpc
{
    public class EquipmentTemplate
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public EquipmentCategory Category { get; set; }
        public bool IsMartial { get; set; }
        public BasicAttackTemplate? BasicAttack { get; set; }
        public StatsModifications? StatsModifier { get; set; }
        public string Quality { get; set; }
        public int? NumHands { get; set; }
    }
}
