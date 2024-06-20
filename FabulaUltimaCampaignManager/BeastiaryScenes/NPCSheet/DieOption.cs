using Godot;

public partial class DieOption : OptionButton
{
    [Export]
    public Godot.Collections.Array<int> DiceSizes { get; set; } = new Godot.Collections.Array<int>() { 6, 8, 10, 12 };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RemoveItem(0);
        foreach (var val in DiceSizes)
        {
            AddItem($"d{val}", val);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
