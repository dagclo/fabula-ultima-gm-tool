using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class GetSpells : VBoxContainer, IBeastAttribute
{
	[Export]
	public PackedScene SpellScene { get; set; }

    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (var child in this.FindChildren("Spell", "PanelContainer"))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        this.Visible = false;
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach (var child in this.FindChildren("*", owned: false).Where(c => c is SpellNode))
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (!beastTemplate.Spells.Any()) return;
        this.Visible = true;
        foreach (var spell in beastTemplate.Spells)
        {
            var scene = SpellScene.Instantiate<SpellNode>();
            scene.SpellObject = spell;
            scene.Beast = beastTemplate;
            this.AddChild(scene);
        }
    }
}
