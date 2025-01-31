using Godot;

public partial class ActionLabel : Label
{
	public void HandleActionSet(string name, string type, string defense, string detail)
	{
		this.Text = name;
	}
}
