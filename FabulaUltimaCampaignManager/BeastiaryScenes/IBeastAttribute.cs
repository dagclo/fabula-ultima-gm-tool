using FabulaUltimaNpc;
using System;
using static BeastEntryNode;
using System.Collections.Generic;

public interface IBeastAttribute
{
    void HandleBeastChanged(IBeastTemplate beastTemplate);
    Action<ISet<BeastEntryNode.Action>> Save { get; set; }
}