using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteQuestion : Label, IBeastAttribute
{
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Text = $"Are you sure you want to delete '{beastTemplate.Name}'?";
    }
}
