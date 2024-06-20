using FabulaUltimaNpc;
using Godot;
using System;

public partial class NameEdit : LineEdit, IBeastAttribute
{
    [Export]
    public string AttributeName { get; set; }

    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        this.Text = beastTemplate.GetAttributeValue(AttributeName);
    }
}
