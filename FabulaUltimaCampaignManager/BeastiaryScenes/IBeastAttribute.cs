using FabulaUltimaNpc;
using System;
using System.Collections.Generic;

public interface IBeastAttribute
{
    void HandleBeastChanged(IBeastTemplate beastTemplate);
    Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }
}