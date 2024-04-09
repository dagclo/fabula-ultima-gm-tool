using FabulaUltimaNpc;
using Godot;

public partial class DescriptionImage : TextureRect, IBeastAttribute
{
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
		if(string.IsNullOrWhiteSpace(beastTemplate.ImageFile)) return;
		var image = Image.LoadFromFile(beastTemplate.ImageFile);
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
    }
}
