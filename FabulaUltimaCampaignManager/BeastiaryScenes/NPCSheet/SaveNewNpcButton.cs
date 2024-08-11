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
        //todo: save when there are no errors
        BeastTemplateAction?.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
	}
}
