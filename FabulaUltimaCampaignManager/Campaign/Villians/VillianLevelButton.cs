using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class VillianLevelButton : OptionButton, INpcReader
{
    private VillainStats _villianStats;

    [Signal]
    public delegate void VillianLevelUpdatedEventHandler(int villainLevel);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Selected = 0;
	}

    public void HandleItemSelected(int index)
    {
        var level = (VillianLevel) GetItemId(index);
        _villianStats.Level = level;
        EmitSignal(SignalName.VillianLevelUpdated, (int) level);
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		_villianStats = npc.VillainStats;
        var index = GetItemIndex((int) _villianStats.Level);
        this.Selected = index;        
    }
}
