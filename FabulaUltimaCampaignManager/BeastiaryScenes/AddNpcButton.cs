using Godot;
using System;

public partial class AddNpcButton : Button
{
    [Export]
    public PackedScene AddNpcScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandlePressed()
	{
		var npcScene = AddNpcScene?.Instantiate<NpcSheet>();
        this.AddChild(npcScene);
        npcScene.Closing += () => OnNpcClose(npcScene);
    }

    private void OnNpcClose(NpcSheet npcScene)
    {
        RemoveChild(npcScene);
        npcScene.QueueFree();
    }
}
