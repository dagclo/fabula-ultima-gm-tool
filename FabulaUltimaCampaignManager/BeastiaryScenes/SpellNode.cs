using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class SpellNode : PanelContainer
{
    public SpellTemplate SpellObject { get; internal set; }
    public IBeastTemplate Beast { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (SpellObject == null) return;
		foreach(var child in this.FindChildren("*", "").Where(c => c is ISpellReader)) 
		{ 
			var spellReader = child as ISpellReader;
			spellReader.Read(SpellObject);
            spellReader.Read(Beast);
        }
	}
}
