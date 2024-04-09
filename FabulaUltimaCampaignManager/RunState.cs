using FirstProject.Campaign;
using FirstProject.Encounters;
using Godot;
using System.Linq;

public partial class RunState : Node
{
    public Encounter RunningEncounter { get; set; }
    public CampaignData Campaign { get; internal set; }
    public bool IsValid => RunningEncounter.NpcCollection.Count > 0 && Campaign.Players.Any(p => !string.IsNullOrWhiteSpace(p.Name));    
}
