using FirstProject.Encounters;
using Godot;
using System;
using System.Linq;

public partial class ArchiveDeleteEncounterButton : Button, IEncounterAttribute
{
	[Export]
	public string ArchiveString { get; set; } = "Archive";

	[Export]
	public string DeleteString { get; set; } = "Delete";

    [Export]
    public Tag ArchiveTag { get; set; }

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
        if(encounter.Tags?.Any(t => t.Name == ArchiveTag.Name) == true)
		{
			this.Text = DeleteString;
		}
		else
		{
			this.Text = ArchiveString;
		}
    }
}


