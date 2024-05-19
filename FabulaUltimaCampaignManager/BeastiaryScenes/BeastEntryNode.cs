using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class BeastEntryNode : PanelContainer
{
	public delegate void BeastChangedEventHandler(IBeastTemplate beastTemplate);

	public event BeastChangedEventHandler BeastChanged;

    private IBeastTemplate _template;    
    private BeastiaryRepository _beastRepository;

    public IBeastTemplate Beast
	{
		set 
		{   
            _template = value;			
			this.BeastChanged?.Invoke(value);
        }        
    }

    public Action<IBeastTemplate> OnAddToEncounter { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        foreach (var child in this.FindChildren("*")            
            .Where(l => l is IBeastAttribute))
        {
            var label = child as IBeastAttribute;
            this.BeastChanged += label.HandleBeastChanged;
            label.Save += this.SaveTemplate;
        }

        if(_template != null) this.BeastChanged?.Invoke(_template);
    }

    private void SaveTemplate()
    {
        _beastRepository?.UpdateBeastTemplate(_template);
        this.BeastChanged?.Invoke(_template);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

	}

    public void OnAddToEncounterButtonPressed()
    {
        if (_template != null) OnAddToEncounter?.Invoke(_template);
    }
}
