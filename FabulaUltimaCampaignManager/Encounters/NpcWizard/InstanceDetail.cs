using Godot;
using System;

public partial class InstanceDetail : PanelContainer, INpcWizardTab
{
    public Action Available { get; set; }

    public string Title => "NPC Details";

    public bool IsReady => true;

    public Action Done { get; set; }

    public void OnNextPressed()
    {
        Done?.Invoke();
    }

    public void OnPrevTabDone()
    {
        Done?.Invoke();
    }
}
