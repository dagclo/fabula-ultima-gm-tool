using FabulaUltimaNpc;
using Godot;
using System;

public partial class UseDiceSizeLabel : Label, IBeastAttribute
{
	[Export]
    public string AttributeDice { get; set; }
    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var diceSize = beastTemplate.GetAttributeValue(AttributeDice);
		this.Text = $"+{diceSize.Substring(1)}";
    }
}
