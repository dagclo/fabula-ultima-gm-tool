using Godot;
using System;

public partial class Help : PopupMenu
{
    [Export]
    public PopupPanel AboutPanel { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnOptionPressed(int index)
	{
		var itemText = GetItemText(index);
		switch(itemText)
		{
			case "About":
				AboutPanel.Show(); 
				break;
		}
	}
}
