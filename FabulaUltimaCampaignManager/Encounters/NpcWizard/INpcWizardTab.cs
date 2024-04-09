using System;

public interface INpcWizardTab
{	
	Action Available { get; set; }
    Action Done { get; set; }
    string Title { get; }	
    void OnPrevTabDone();
    bool IsReady { get; }
}
