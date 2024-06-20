using FabulaUltimaDatabase.Models;
using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class NpcSheet : Window
{
    public Action Closing { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (var child in this.FindChildren("*", recursive: false)
           .Where(l => l is BeastEntryNode))
        {
            var label = child as BeastEntryNode;
            var beast = new BeastModel();
            label.Beast = new BeastTemplate(beast);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleCloseRequested()
	{
		Closing?.Invoke();
	}
}
