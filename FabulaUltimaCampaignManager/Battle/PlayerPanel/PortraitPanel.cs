using Godot;
using System;
using FirstProject.Campaign;

public partial class PortraitPanel : PanelContainer
{

	[Export]
    public Color DisabledColor { get; set; } = Color.FromHtml("#808080");

	[Export]
    public Color EnabledColor { get; set; } = Colors.White;

	public void ReadPlayer(PlayerData player)
    {
        if(player.IsActive){
			this.Modulate = EnabledColor;
		}
		else
		{
			Modulate = DisabledColor;
		}
    }  
}
