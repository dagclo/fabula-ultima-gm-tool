using Godot;

public partial class CompleteTurnButton : Button
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnRoundStart()
	{
        this.Disabled = false;
    }

    public void OnRoundEnd()
    {
        Disabled = true;
    }

    public void OnTurnStart()
    {			
		this.Disabled = false;
    }

	public void OnButtonPressed()
	{
		this.Disabled = true;
    }
}
