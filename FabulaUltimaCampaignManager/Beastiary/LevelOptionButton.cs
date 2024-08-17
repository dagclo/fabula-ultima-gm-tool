using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System.Linq;

public partial class LevelOptionButton : OptionButton
{

    [Export]
    public int Min { get; set; } = 5;

    [Export]
    public int Max { get; set; } = 60;

    [Export]
    public int Multiple { get; set; } = 5;

    [Signal]
    public delegate void UpdateBeastFilterEventHandler(SignalWrapper<ISearchFilter<IBeastTemplate>> addFilter, SignalWrapper<ISearchFilter<IBeastTemplate>> removeFilter);
    private ISearchFilter<IBeastTemplate> _currentFilter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddItem("", 0);
        foreach (var level in Enumerable.Range(Min, (Max - Min) + 1).Where(i => i % 5 == 0))
        {
            AddItem(level.ToString(), level);
        }
    }

    public void OnItemSelected(int index)
    {
        if (index == 0)
        {
            EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(null), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
            _currentFilter = null;
            return;
        }
        var level = GetItemId(index);
        var nextFilter = new SearchFilter<IBeastTemplate>((b) => b.Level == level);
        EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(nextFilter), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
        _currentFilter = nextFilter;
    }
}
