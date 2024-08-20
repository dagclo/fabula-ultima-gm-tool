using Godot;
using System;

public partial class CampaignManager : TabContainer
{
	[Export]
	public int DefaultStartTab { get; set; } = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.CurrentTab = Math.Clamp(DefaultStartTab, 0, this.GetTabCount() - 1);
	}
}
