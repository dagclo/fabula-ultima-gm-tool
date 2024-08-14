using Godot;
using System;

namespace FirstProject.Campaign
{
    public partial class PlayerData : Resource
    {
        [Export]
        public string Name { get; set; }
        public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Enabled;

        [Export]
        public string CharacterName { get; set; }

        [Export]
        public string CharacterTitle { get; set; }

        [Export]
        public string PortraitFile { get; set; }

        [Export]
        public bool Enabled { get; set; } = false;

        public Action<bool> ActiveChanged { get; set; }
        private bool _isActive = true;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                var oldValue = _isActive;                
                _isActive = value;
                if (oldValue != value) ActiveChanged.Invoke(value);
            }
        }
    }
}
