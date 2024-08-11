using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SaveNewNpcButton : Button, IBeastAttribute
{
    private int? _errorCount;
    private int? _warningCount;

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        // do nothing for now
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
            BeastEntryNode.Action.CHANGED
        };
        if (_errorCount == 0) actions.Add(BeastEntryNode.Action.SAVE);
        BeastTemplateAction?.Invoke(actions);
	}
}
