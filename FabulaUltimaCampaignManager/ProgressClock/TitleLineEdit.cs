using Godot;
using System;

public partial class TitleLineEdit : LineEdit
{
	public void Initialize(string title)
	{
		this.Text = title;
	}
}
