using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class ResultDetail : PanelContainer, INpcWizardTab, INpcReader
{
    private NpcInstance _instance;

    public Action Available { get; set; }

    public string Title => "Results";

    public bool IsReady => false;

    public Action Done { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
    }

    public void OnPrevTabDone()
    {
        Available?.Invoke();
    }
}
