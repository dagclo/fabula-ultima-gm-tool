using FabulaUltimaNpc;
using Godot;

public partial class SpellTarget : Label, ISpellReader
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
		this.Text = spellTemplate.Target;
    }

    public void Read(IBeastTemplate beast)
    {
        // do nothing

    }
}
