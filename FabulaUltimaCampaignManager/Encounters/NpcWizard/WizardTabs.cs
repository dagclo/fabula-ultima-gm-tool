using Godot;

public partial class WizardTabs : TabContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var tabIndex = -1;
        INpcWizardTab lastValidTab = null;
        foreach (var node in this.GetChildren())
        {
            tabIndex++;
            if (!(node is INpcWizardTab tab)) continue;
            var currentIndex = tabIndex;
            tab.Available += () =>  OnTabAvailable(currentIndex);
            //SetTabHidden(tabIndex, true);
            SetTabTitle(tabIndex, tab.Title);
            if(lastValidTab != null)
            {
                lastValidTab.Done += tab.OnPrevTabDone;
            }
            if (tab.IsReady)
            {
                SetTabHidden(tabIndex, false);
            }
            lastValidTab = tab;
            
        }
        CurrentTab = 0;
    }

    private void OnTabAvailable(int tabIndex)
    {
        SetTabHidden(tabIndex, false);
        if(CurrentTab != tabIndex)
        {
            CurrentTab = tabIndex;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
