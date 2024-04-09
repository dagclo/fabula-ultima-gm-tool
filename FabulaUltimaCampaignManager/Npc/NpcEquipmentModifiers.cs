using FabulaUltimaNpc;
using Godot;

namespace FirstProject.Npc
{
    public partial class NpcEquipmentModifiers : Resource
    {
        public NpcEquipmentModifiers() : this(new StatsModifications())
        {

        }

        public NpcEquipmentModifiers(StatsModifications statsModifications)
        {
            StatsModifications = statsModifications;
        }

        public StatsModifications StatsModifications { get; set; }

        [Export]
        public int InitiativeModifier
        {
            get
            {
                return StatsModifications.InitiativeModifier;
            }
            set
            {
                StatsModifications.InitiativeModifier = value;
            }
        }

        [Export]
        public int MagicDefenseModifier
        {
            get
            {
                return StatsModifications.MagicDefenseModifier;
            }
            set
            {
                StatsModifications.MagicDefenseModifier = value;
            }
        }

        [Export]
        public int DefenseModifier
        {
            get
            {
                return StatsModifications.DefenseModifier;
            }
            set
            {
                StatsModifications.DefenseModifier = value;
            }
        }

        [Export]
        public bool DefenseOverrides
        {
            get
            {
                return StatsModifications.DefenseOverrides;
            }
            set
            {
                StatsModifications.DefenseOverrides = value;
            }
        }
    }
}