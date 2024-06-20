using FabulaUltimaNpc;
using Godot;
using System;

public partial class SpeciesSelect : OptionButton, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    public Action<bool> Save { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0);
        foreach (var val in new[] { "Beast", "Construct", "Demon", "Elemental", "Humanoid", "Monster", "Plant", "Undead" })
        {
            AddItem(val);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
    }
}
