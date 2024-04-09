using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class NextButton : Button, INpcReader
{
    private NpcInstance _instance;

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
		_instance.Changed += OnInstanceChanged;
    }

    private void OnInstanceChanged()
    {
        Disabled = true;
        if (string.IsNullOrWhiteSpace(_instance.InstanceName)) return;
        if (_instance.Model.Level < _instance.Template.Level) return;
        Disabled = false;
    }
}
