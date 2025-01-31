using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class UltimaOptionButton : OptionButton, INpcReader
{
    private NpcInstance _instance;

    [Export]
    public int Min { get; set; } = 0;
    private int _minIndex;

    [Export]
    public int Max { get; set; } = 15;

    [Export]
    public int Multiple { get; set; } = 1;    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void OnItemSelected(int index)
    {
        var level = GetItemId(index);
        _instance.VillainStats.UltimaPoints = level;        
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
        Max = (int) npc.VillainStats.Level;
        UpdateItems();

        var index = GetItemIndex(_instance.VillainStats.UltimaPoints);
        this.Selected = index > -1 ? index : _minIndex;
    }

    private void UpdateItems()
    {
        Clear();
        foreach (var pointCount in Enumerable.Range(Min, (Max - Min) + 1).Where(i => i % Multiple == 0))
        {
            AddItem(pointCount.ToString(), pointCount);
        }
        _minIndex = GetItemIndex(Min);
    }

    public void HandleVillianLevelUpdated(int ultimaPoints)
    {
        Max = ultimaPoints;
        UpdateItems();
        var index = GetItemIndex(ultimaPoints);
        this.Selected = index > -1 ? index : _minIndex;
        _instance.VillainStats.UltimaPoints = ultimaPoints;
    }
}
