using FabulaUltimaNpc;
using Godot;
using System;

public partial class ComputedHealthLabel : Label, IBeastAttribute
{
    public Action Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (beastTemplate == null) return;     
		this.Text = $"{beastTemplate.HealthPoints} | {beastTemplate.Crisis}";
	}
}
