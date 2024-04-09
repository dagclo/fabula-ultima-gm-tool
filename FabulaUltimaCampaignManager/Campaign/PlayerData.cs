using Godot;
using System;

namespace FirstProject.Campaign
{
    public partial class PlayerData : Resource
    {
        [Export]
        public string Name { get; set; }
        public bool IsValid => !string.IsNullOrWhiteSpace(Name);

        [Export]
        public string CharacterName { get; set; }

        [Export]
        public string CharacterTitle { get; set; }

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
