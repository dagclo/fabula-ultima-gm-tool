using Godot;

namespace FirstProject.Encounters
{
    public partial class InitiativeSeed : Resource
    {
        private int _npcInitiative = -1;
        [Export]
        public int NpcInitiative 
        {
            get
            {
                return _npcInitiative;
            }
            set
            {
                _npcInitiative = value;
                this.EmitChanged();
            }
        }

        private int _playerCheck = -1;
        [Export]        
        public int PlayerCheck
        {
            get
            {
                return _playerCheck;
            }
            set
            {
                _playerCheck = value;
                this.EmitChanged();
            }
        }

        public bool IsValid => NpcInitiative > 0 && PlayerCheck > 0;
        public bool PlayersWon => PlayerCheck >= NpcInitiative;

        public int NumPlayers { get; internal set; }
    }
}
