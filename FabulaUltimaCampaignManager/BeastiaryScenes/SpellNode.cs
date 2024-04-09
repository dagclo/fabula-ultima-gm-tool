using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class SpellNode : PanelContainer
{
    public SpellTemplate SpellObject { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (SpellObject == null) return;
		foreach(var child in this.FindChildren("*", "Label").Where(c => c is ISpellReader)) 
		{ 
			var spellReader = child as ISpellReader;
			spellReader.Read(SpellObject);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
