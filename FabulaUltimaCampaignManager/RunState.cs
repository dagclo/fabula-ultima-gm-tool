using FirstProject.Campaign;
using FirstProject.Encounters;
using Godot;
using System;
using System.Linq;

public partial class RunState : Node
{
    public Encounter RunningEncounter { get; set; }
    public CampaignData Campaign { get; internal set; }
    public bool HasNpcs => RunningEncounter?.NpcCollection?.Count > 0;
    // p.IsValid (name AND enabled) matches what InitiativePopup counts, so a
    // named-but-disabled player can't start a battle with zero participants
    public bool HasPlayers => Campaign?.Players?.Any(p => p.IsValid) == true;
    public bool IsValid => HasNpcs && HasPlayers;

    private double _volumeLevel;
    public double VolumeLevel
    {
        get => _volumeLevel;
        set
        {
            _volumeLevel = value;
            VolumeLevelChanged?.Invoke(_volumeLevel);
        }
    }
    public Action<double> VolumeLevelChanged { get; set; }
}
