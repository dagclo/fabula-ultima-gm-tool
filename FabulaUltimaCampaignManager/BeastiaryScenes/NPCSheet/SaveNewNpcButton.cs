using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;

public partial class SaveNewNpcButton : Button, IBeastAttribute
{
    private int? _errorCount;
    private int? _warningCount;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var npcModel = beastTemplate.Model as NpcModel;
        if (npcModel != null)
        {
            this.Text = "Add Copy To Scene";
        }
    }

    public void HandleErrorsAndWarnings(int errorCount, int warningCount)
	{
		_errorCount = errorCount;
		_warningCount = warningCount;
	}

	public void HandlePressed()
    {
        var actions = new HashSet<BeastEntryNode.Action>
        {
            BeastEntryNode.Action.TRIGGER
        };
        if (_errorCount == 0) actions.Add(BeastEntryNode.Action.SAVE);
        BeastTemplateAction?.Invoke(actions);
	}
}
