using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
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
    public Action<IBeastTemplate> OnDeleteBeast { get; set; }
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        foreach (var child in this.FindChildren("*")            
            .Where(c => c is IBeastAttribute))
        {
            var beastAttr = child as IBeastAttribute;
            this.BeastChanged += beastAttr.HandleBeastChanged;
            beastAttr.Save += this.SaveTemplate;
        }

        if(_template != null) this.BeastChanged?.Invoke(_template);
    }

    private void SaveTemplate(ISet<Action> actions)
    {
        if (actions.Contains(Action.DELETE))
        {
            this.OnDeleteBeast?.Invoke(_template);
            return;
        }
        
        if(actions.Contains(Action.SAVE))
        {
            _beastRepository?.UpdateBeastTemplate(_template);            
        }

        if(actions.Contains(Action.TRIGGER)) this.BeastChanged?.Invoke(_template);
    }

    public void OnAddToEncounterButtonPressed()
    {
        if (_template != null) OnAddToEncounter?.Invoke(_template);
    }

    public enum Action
    {
        SAVE,
        TRIGGER,
        DELETE,
    }
}
