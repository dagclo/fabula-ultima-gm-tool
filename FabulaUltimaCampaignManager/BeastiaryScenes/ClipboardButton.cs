using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public partial class ClipboardButton : Button, IBeastAttribute
{
    private IBeastTemplate _template;

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _template = beastTemplate;
    }

    public void OnClick()
    {
        var beastText = _template.ToText();
        DisplayServer.ClipboardSet(beastText);
    }
}
