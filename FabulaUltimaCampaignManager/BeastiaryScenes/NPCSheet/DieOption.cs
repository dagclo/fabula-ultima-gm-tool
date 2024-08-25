using Godot;

public partial class DieOption : OptionButton
{
    [Export]
    public Godot.Collections.Array<int> DiceSizes { get; set; } = new Godot.Collections.Array<int>() { 6, 8, 10, 12 };

    [Signal]
    public delegate void DieSizeChangedEventHandler(int size);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {   
        foreach (var val in DiceSizes)
        {
            AddItem($"d{val}", val);
        }
        Select(0);
    }

    public void OnSelected(int index)
    {
        var size = GetItemId(index);
        EmitSignal(SignalName.DieSizeChanged, size);
    }

    public void HandleAttributeSizeSet(int size)
    {
        Select(GetItemIndex(size));
    }
}
