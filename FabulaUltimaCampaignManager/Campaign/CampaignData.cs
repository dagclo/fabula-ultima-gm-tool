using FirstProject.Encounters;
using Godot;
using System;
using System.Linq;

namespace FirstProject.Campaign
{
    public partial class CampaignData : Resource
    {
        private string _name;
        [Export]
        public string Name
        {
            get => _name;
            set
            {
                if(_name ==  value) return;
                _name = value;
                EmitChanged();
            }
        }

        [Export]
        public Godot.Collections.Array<Encounter> Encounters { get; set; }

        [Export]
        public string Id { get; set; }

        [Export]
        public Godot.Collections.Array<PlayerData> Players { get; set; }

        public CampaignData() : this(
            Guid.Empty.ToString(), 
            "", 
            new Godot.Collections.Array<Encounter>(), 
            new Godot.Collections.Array<PlayerData>()) { }

        public CampaignData(
            string id, 
            string name, 
            Godot.Collections.Array<Encounter> encounters,
            Godot.Collections.Array<PlayerData> players) 
        {
            _name = name;
            Encounters = encounters;            
            Id = id;
            Players = players;
        }

        internal void EnsurePlayerSlot(int slotIndex)
        {
            lock(Players)
            {
                var slotsNeeded = slotIndex + 1;
                var slotsAvailable = Players.Count;
                var slotsToAdd = Math.Clamp(slotsNeeded - slotsAvailable, 0, slotsNeeded);                
                foreach (var _ in Enumerable.Range(0, slotsToAdd))
                {
                    Players.Add(new PlayerData());
                }
            }            
        }
    }
}
