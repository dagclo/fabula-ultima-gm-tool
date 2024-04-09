using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class Name : Label, INpcReader
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
        _instance.Changed += OnChanged;
    }

    private void OnChanged()
    {
        Text = $"Name: {_instance.InstanceName}";
    }
}
