using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class SpellList : VBoxContainer, INpcReader, INpcStatusReader
{
    [Export]
    public PackedScene SpellScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
        foreach (var child in this.FindChildren("SpellPanel", "PanelContainer"))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (!npc.Template.Spells.Any()) return;
        var beastTemplate = npc.Template;
        foreach (var spell in beastTemplate.Spells)
        {
            var scene = SpellScene.Instantiate<SpellPanel>();
            this.AddChild(scene);
            scene.UpdateSpell(spell, npc);
        }
    }

    public void HandleStatusSet(BattleStatus status)
    {
        foreach (var spellPanel in this.GetChildren().Select(c => (SpellPanel) c))
        {
            spellPanel.UpdateStatus(status);
        }
    }
}
