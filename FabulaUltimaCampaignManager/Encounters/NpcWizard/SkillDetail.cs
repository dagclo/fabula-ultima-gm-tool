using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class SkillDetail : PanelContainer, INpcWizardTab, INpcReader
{
    private NpcInstance _instance;
    private int _numSkills;

    public Action Available { get; set; }

    public string Title => "Set Skills";
    public bool IsReady => _numSkills > 0;

    public Action Done { get; set; }

    [Signal]
    public delegate void SkillsReadyEventHandler(int numSkills);

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
        if(_instance != null)
        {
            _instance.Changed -= OnInstanceChanged;
        }
        _instance = npc;
        _instance.Changed += OnInstanceChanged;
    }

    private void OnInstanceChanged()
    {
        _numSkills = _instance.Model.Rank.GetNumSkills();
    }

    public void OnNextPressed()
    {
        Done?.Invoke();
    }

    public void OnPrevTabDone()
    {
        if (_numSkills > 0)
        {
            Available?.Invoke();            
        }
        else
        {
            Done?.Invoke();
        }
        EmitSignal(SignalName.SkillsReady, _numSkills);
    }
}
