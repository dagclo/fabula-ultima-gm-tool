using FirstProject.Encounters;
using Godot;
using System;
using System.Linq;


public partial class EncounterEntry : VBoxContainer
{
    public delegate void EncounterChangedEventHandler(Encounter encounter);

    public event EncounterChangedEventHandler EncounterChanged;

    private Encounter _encounter = null;

    public Action OnSave { get; set; }
    public Action<Encounter, EncounterEntry>  OnDeleteArchiveEncounter { get; set; }
    public Action<Encounter> OnLoadEncounter { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (var child in this.FindChildren("*")
           .Where(l => l is IEncounterAttribute))
        {
            var label = child as IEncounterAttribute;
            this.EncounterChanged += label.HandleEncounterChanged;
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetEncounter(Encounter encounter)
	{
        if (encounter == null) return;
        _encounter = encounter;
        this.EncounterChanged?.Invoke(_encounter);
        _encounter.Changed += EncounterInstanceChanged;
    }

    private void EncounterInstanceChanged()
    {
        this.EncounterChanged?.Invoke(_encounter);
        OnSave?.Invoke();
    }

    private void OnDeleteButtonPressed()
    {
        OnDeleteArchiveEncounter?.Invoke(_encounter, this);
    }

    private void OnLoadButtonPressed()
    {        
        OnLoadEncounter?.Invoke(_encounter);
    }
}
