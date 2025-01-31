using Godot;

public partial class NewCampaignButton : Button
{
	public void HandleTextValid(bool isValid)
	{
		this.Disabled = !isValid;
	}
}
