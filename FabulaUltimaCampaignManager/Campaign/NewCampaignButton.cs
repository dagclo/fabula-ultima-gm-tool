using Godot;
using System;

public partial class NewCampaignButton : Button
{
	public void HandleTextValid(bool isValid)
	{
		this.Disabled = !isValid;
	}
}
