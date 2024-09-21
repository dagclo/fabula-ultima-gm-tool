using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelEdit : OptionButton, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public int Min { get; set; } = 5;

    [Export]
    public int Max { get; set; } = 60;

    [Export]
    public int Multiple { get; set; } = 5;
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{   
        foreach (var level in Enumerable.Range(Min, (Max - Min) + 1).Where(i => i % Multiple == 0))
        {
            AddItem(level.ToString(), level);
        }
    }

    public void OnItemSelected(int index)
    {
        var level = GetItemId(index);
        _beastTemplate.Level = level;
        BeastTemplateAction.Invoke(new HashSet<BeastEntryNode.Action>() { BeastEntryNode.Action.TRIGGER });
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _beastTemplate = beastTemplate;
        var index = GetItemIndex(_beastTemplate.Level);
        this.Selected = index;
    }
}
