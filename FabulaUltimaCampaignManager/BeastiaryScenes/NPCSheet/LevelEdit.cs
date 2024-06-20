using Godot;
using System.Linq;

public partial class LevelEdit : OptionButton
{
    [Export]
    public int Min { get; set; } = 5;

    [Export]
    public int Max { get; set; } = 60;

    [Export]
    public int Multiple { get; set; } = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        RemoveItem(0);
        foreach (var level in Enumerable.Range(Min, (Max - Min) + 1).Where(i => i % 5 == 0))
        {
            AddItem(level.ToString(), level);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
