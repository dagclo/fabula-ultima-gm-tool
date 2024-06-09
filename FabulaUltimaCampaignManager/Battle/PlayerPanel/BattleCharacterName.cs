using FirstProject.Campaign;
using Godot;
using System;

public partial class BattleCharacterName : Label
{
    [Export]
    public Color DisabledColor { get; set; } = Color.FromHtml("#808080");

    public void ReadPlayer(PlayerData player)
    {
        this.Text = player.CharacterName;
        this.SetFontColor(player.IsActive, DisabledColor);
    }    
}
