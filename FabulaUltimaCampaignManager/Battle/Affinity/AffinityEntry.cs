using FabulaUltimaSkillLibrary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class AffinityEntry : HBoxContainer, INpcReader
{
    [Export]
    public string TargetDamageType { get; set; } = "Dark";

    public void HandleNpcChanged(NpcInstance npc)
    {
        var affinity = npc.Template.Resistances[TargetDamageType.ToLowerInvariant()];
        this.Visible = affinity.AffinityId != DamageConstants.NO_AFFINITY;
        foreach (var child in this.FindChildren("AffinityValue")
          .Where(l => l is Label))
        {
            var label = child as Label;
            label.Text = affinity.Affinity;
        }
    }
}
