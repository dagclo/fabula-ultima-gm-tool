using FirstProject.Encounters;
using Godot;

public partial class EncounterTitleEdit : LineEdit
{
    private Encounter _encounter;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateEncounter(Encounter encounter)
    {
        if (encounter == null) return;
        _encounter = encounter;       
        this.Text = _encounter.Name;
    }

    public void OnTextSubmitted(string newText)
    {
       if(_encounter == null) return;        
       _encounter.Name = newText;
    }
}
