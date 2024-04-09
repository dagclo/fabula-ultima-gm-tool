using FirstProject.Encounters;
using Godot;
using System;

public partial class GroupCheck : LineEdit, IInitiativeSeedReader
{
    private InitiativeSeed _initiativeSeed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnInitiativeSeedReady(InitiativeSeed seed)
    {
        _initiativeSeed = seed;
    }

    public void OnTextChanged(string newText) 
    {
        if (_initiativeSeed == null) throw new Exception("not set");
        _initiativeSeed.PlayerCheck = int.TryParse(newText, out var checkNum) ? checkNum : -1; // maybe send up to some sort of error num
    }
}
