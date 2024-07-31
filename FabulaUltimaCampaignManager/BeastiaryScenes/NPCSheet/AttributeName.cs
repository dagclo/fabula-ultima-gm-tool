using Godot;

public partial class AttributeName : Label
{
	public void HandleNameChanged(string name)
	{
		this.Text = $"{name}:";
	}
}
