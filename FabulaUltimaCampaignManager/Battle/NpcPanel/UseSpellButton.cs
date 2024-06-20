using Godot;

public partial class UseSpellButton : Button
{	
	public void OnNotEnoughMP()
	{
        this.Disabled = true;
    }
    
}
