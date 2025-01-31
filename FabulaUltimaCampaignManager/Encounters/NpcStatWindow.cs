using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NpcStatWindow : Window
{
    private NpcInstance _instance;

    private BeastEntryNode _beastEntyNode { get; set; }

    public Action OnClose { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{        
        foreach (var child in this.FindChildren("*")
         .Where(l => l is BeastEntryNode))
        {
            _beastEntyNode = child as BeastEntryNode;
            _beastEntyNode.BeastChanged += HandleBeastChanged;
        }
        this.ResizeForResolution();
    }

    private void HandleBeastChanged(IBeastTemplate _)
    {
        if (_instance == null) return;
        Title = $"{_instance.Model.Rank}: {_instance.InstanceName}";
    }

    public void SetBeast(NpcInstance instance)
	{     
        _beastEntyNode.Beast = instance.Template;
        _instance = instance;
        Title = $"{_instance.Model.Rank}: {_instance.InstanceName}";
	}

    public void OnCloseRequested()
    {
        this.Hide();
        OnClose.Invoke();
    }

}
