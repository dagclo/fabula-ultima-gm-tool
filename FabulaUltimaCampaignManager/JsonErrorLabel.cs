using Godot;
using System;

public partial class JsonErrorLabel : Label
{
	public void HandleErrorFound(string error)
	{
		this.Text = error;
	}
}