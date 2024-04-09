using FirstProject.Campaign;
using Godot;

public partial class BattlePlayerName : Label
{
    [Export]
    public Color DisabledColor { get; set; } = Color.FromHtml("#808080");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ReadPlayer(PlayerData player)
	{
		this.Text = player.Name;
        this.SetFontColor(player.IsActive, DisabledColor);
    }
}
