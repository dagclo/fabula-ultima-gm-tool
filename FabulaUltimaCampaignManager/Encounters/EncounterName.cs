using FirstProject.Encounters;
using Godot;

public partial class EncounterName : Label, IEncounterAttribute
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleEncounterChanged(Encounter encounter)
    {
        this.Text = encounter.Name;
    }
}
