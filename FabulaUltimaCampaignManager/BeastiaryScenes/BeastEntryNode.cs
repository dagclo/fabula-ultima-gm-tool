using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BeastEntryNode : Container
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
    public Action<IBeastTemplate> OnTrigger { get; set; }
    public System.Action OverrideSave { get; internal set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        foreach (var child in this.FindChildren("*")            
            .Where(c => c is IBeastAttribute))
        {
            var beastAttr = child as IBeastAttribute;
            this.BeastChanged += beastAttr.HandleBeastChanged;
            beastAttr.BeastTemplateAction += this.ActionTemplate;
        }

        if(_template != null) this.BeastChanged?.Invoke(_template);
    }

    public void ActionTemplate(ISet<Action> actions)
    {
        if (actions.Contains(Action.DELETE))
        {
            this.OnDeleteBeast?.Invoke(_template);
            return;
        }
        
        if(actions.Contains(Action.CHANGED)) this.BeastChanged?.Invoke(_template);
        if(actions.Contains(Action.TRIGGER)) this.OnTrigger?.Invoke(_template);
        if (actions.Contains(Action.SAVE))
        {            
            if(OverrideSave != null)
            {
                OverrideSave();
            }
            else
            {
                _beastRepository?.RunQueuedUpdates(_template.Id);
                _beastRepository?.UpdateBeastTemplate(_template);
            }            
        }
    }

    public void OnAddToEncounterButtonPressed()
    {
        if (_template != null) OnAddToEncounter?.Invoke(_template);
    }

    public enum Action
    {
        SAVE,
        CHANGED,
        DELETE,
        TRIGGER
    }
}
