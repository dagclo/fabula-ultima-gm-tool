using Godot;
using System;

public partial class PortraitFileDialog : Godot.FileDialog
{

	public void HandleSelectImageFile()
	{
		this.Visible = true;
	}
}
