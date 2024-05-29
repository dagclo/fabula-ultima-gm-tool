using FabulaUltimaNpc;
using Godot;
using System;

public partial class DeleteQuestion : Label, IBeastAttribute
{
    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Text = $"Are you sure you want to delete '{beastTemplate.Name}'?";
    }
}
