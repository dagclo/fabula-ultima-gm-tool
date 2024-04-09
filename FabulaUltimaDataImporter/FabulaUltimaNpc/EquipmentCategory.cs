namespace FabulaUltimaNpc
{
    public class EquipmentCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsWeapon { get; set; }
        public bool IsArmor { get; set; }

        public bool IsRanged { get; set; }
    }
}