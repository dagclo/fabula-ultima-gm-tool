using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject;
using Godot;
using System;
using System.Linq;

public partial class SpeciesOptionButton : OptionButton
{
    [Signal]
    public delegate void UpdateBeastFilterEventHandler(SignalWrapper<ISearchFilter<IBeastTemplate>> addFilter, SignalWrapper<ISearchFilter<IBeastTemplate>> removeFilter);
    private ISearchFilter<IBeastTemplate> _currentFilter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        AddItem("", 0);
        foreach (var val in beastRepository.Database.GetSpecies().Select(s => s.Name))
        {
            AddItem(val);
        }
        this.Selected = 0;
    }

    public void HandleItemSelect(int index)
    {
        if(index == 0)
        {
            EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(null), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
            _currentFilter = null;
            return;
        }
        var speciesName = GetItemText(index);
        var nextFilter = new SearchFilter<IBeastTemplate>((b) => b.Species.Name.Equals(speciesName, StringComparison.InvariantCultureIgnoreCase));
        EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(nextFilter), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
        _currentFilter = nextFilter;
    }
}
