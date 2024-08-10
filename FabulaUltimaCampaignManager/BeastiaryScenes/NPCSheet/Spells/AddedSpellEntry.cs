using FabulaUltimaNpc;
using Godot;
using System;

public partial class AddedSpellEntry : VBoxContainer
{
    public SpellTemplate Spell { get; internal set; }
    public Action<AddedSpellEntry> OnRemoveSpell { get; internal set; }
    public Action OnUpdateBeast { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        
    }
}
