using Godot;
using System;

public partial class ActionLabel : Label
{
	public void HandleActionSet(string name, string type, string defense)
	{
		this.Text = name;
	}
}
