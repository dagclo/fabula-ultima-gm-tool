using FabulaUltimaNpc;
using Godot;

public partial class SpellOffensive : Label , ISpellReader
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void Read(SpellTemplate spellTemplate)
    {
        if(spellTemplate.IsOffensive)
		{
			this.Text = "[Off]";
		}
		else
		{
			this.Text = string.Empty;
		}
    }
}
