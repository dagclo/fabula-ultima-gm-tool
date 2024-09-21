using Godot;

namespace FirstProject.Npc
{
    public partial class VillainStats : Resource
    {
        private int _ultimaPoints = 5;
        [Export]
        public int UltimaPoints
        {
            get => _ultimaPoints;
            set
            {
                var changed =_ultimaPoints != value;
                _ultimaPoints = value;
                if(changed) EmitChanged();
            }
        }

        private VillianLevel _villianLevel = VillianLevel.Minor;
        [Export]
        public VillianLevel Level
        {
            get => _villianLevel;
            set
            {
                var changed = _villianLevel != value;
                _villianLevel = value;
                if (changed) EmitChanged();
            }
        }
    }

    public enum VillianLevel
    {
        Minor = 5,
        Major = 10,
        Supreme = 15,
    }
}