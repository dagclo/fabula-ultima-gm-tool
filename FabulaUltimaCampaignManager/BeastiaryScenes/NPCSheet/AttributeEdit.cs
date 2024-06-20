using FabulaUltimaNpc;
using Godot;
using System;

public partial class AttributeEdit : Control, IBeastAttribute
{
    private IBeastTemplate _beastTemplate;

    [Export]
    public string AttributeName { get; set; }

    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;
        _beastTemplate = beastTemplate;
    }
}
