using Godot;

public partial class Help : PopupMenu
{
    [Export]
    public PopupPanel AboutPanel { get; set; }

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
