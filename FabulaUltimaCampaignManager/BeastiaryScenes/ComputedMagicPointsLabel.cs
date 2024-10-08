using FabulaUltimaNpc;
using Godot;
using System;

public partial class ComputedMagicPointsLabel : Label, IBeastAttribute
{
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        this.Text = beastTemplate.MagicPoints.ToString();
    }
}
