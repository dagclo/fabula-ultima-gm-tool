using FabulaUltimaNpc;
using Godot;

public partial class UseDiceSizeLabel : Label, IBeastAttribute
{
	[Export]
    public string AttributeDice { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        var diceSize = beastTemplate.GetAttributeValue(AttributeDice);
		this.Text = $"+{diceSize.Substring(1)}";
    }
}
