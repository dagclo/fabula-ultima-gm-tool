using Godot;

public partial class ImageResistanceReceiver : TextureRect, IResistanceReceiver
{
    [Export]
    public Texture2D BrightImage { get; set; }

    [Export]
    public Texture2D GrayImage { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Texture = GrayImage ?? this.Texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleResistanceChanged(string affinity)
    {
        if (string.IsNullOrWhiteSpace(affinity)) this.Texture = GrayImage ?? this.Texture;
        else this.Texture = BrightImage ?? this.Texture;
    }
}
