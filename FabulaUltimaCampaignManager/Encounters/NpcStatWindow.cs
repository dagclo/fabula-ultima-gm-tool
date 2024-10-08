using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcStatWindow : Window
{
    private BeastEntryNode _beastEntyNode { get; set; }

    public Action OnClose { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{        
        foreach (var child in this.FindChildren("*")
         .Where(l => l is BeastEntryNode))
        {
            _beastEntyNode = child as BeastEntryNode;            
        }
        this.ResizeForResolution();
    }

	public void SetBeast(NpcInstance instance)
	{     
        _beastEntyNode.Beast = instance.Template;
        Title = $"{instance.Model.Rank}: {instance.InstanceName}";
	}

    public void OnCloseRequested()
    {
        this.Hide();
        OnClose.Invoke();
    }

}
