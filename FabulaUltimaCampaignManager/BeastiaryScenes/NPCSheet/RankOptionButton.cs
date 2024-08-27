using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RankOptionButton : OptionButton, IBeastAttribute
{
    private NpcModel _npcModel;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Visible = false;
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _npcModel = beastTemplate.Model as NpcModel;
        if (_npcModel == null) return;
        this.Visible = true;
    }

    public void HandleSelected(int index)
    {
        var id = GetItemId(index);
        var rank = (FabulaUltimaNpc.Rank)id;
        _npcModel.Rank = rank;
        BeastTemplateAction.Invoke(new[] { BeastEntryNode.Action.TRIGGER }.ToHashSet());
    }
}
