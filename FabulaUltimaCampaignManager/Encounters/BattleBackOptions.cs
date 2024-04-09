using FirstProject.Encounters;
using Godot;
using System;

public partial class BattleBackOptions : OptionButton
{
    private Encounter _encounter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (FirstProject.Encounters.Background background in (FirstProject.Encounters.Background[])Enum.GetValues(typeof(FirstProject.Encounters.Background)))
        {
            AddItem(background.ToString(), (int) background);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateEncounter(Encounter encounter)
    {
        if (encounter == null) return;
        _encounter = encounter;
        this.Select((int)encounter.Background);
    }

    public void OnSelection(int index)
    {
        _encounter.Background = (FirstProject.Encounters.Background) index;
    }
}
