using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SearchBeastsTextEdit : LineEdit
{
    [Signal]
    public delegate void UpdateBeastFilterEventHandler(SignalWrapper<ISearchFilter<IBeastTemplate>> addFilter, SignalWrapper<ISearchFilter<IBeastTemplate>> removeFilter);

	private ISearchFilter<IBeastTemplate> _noFilter = new SearchFilter<IBeastTemplate>((b) => true);
    private ISearchFilter<IBeastTemplate> _currentFilter;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

	}

    public void OnTextSubmitted(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
        {
            EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(_noFilter), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
            _currentFilter = _noFilter;
        }
        else
        {
            var nameFilter = new SearchFilter<IBeastTemplate>((b) => b.Name?.Contains(newText, System.StringComparison.InvariantCultureIgnoreCase) == true);
            EmitSignal(SignalName.UpdateBeastFilter, new SignalWrapper<ISearchFilter<IBeastTemplate>>(nameFilter), new SignalWrapper<ISearchFilter<IBeastTemplate>>(_currentFilter));
            _currentFilter = nameFilter;
        }
    }
}
