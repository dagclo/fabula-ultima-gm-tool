using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AddedSpellEntry : VBoxContainer
{
    [Signal]
    public delegate void SpellSetEventHandler(SignalWrapper<SpellTemplate> spell);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> beast);
    public SpellTemplate Spell { get; internal set; }
    public Action<AddedSpellEntry> OnRemoveSpell { get; internal set; }
    public Action OnUpdateBeast { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (Spell == null) return;
        EmitSignal(SignalName.SpellSet, new SignalWrapper<SpellTemplate>(Spell));
	}


    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveSkill()
    {
        OnRemoveSpell?.Invoke(this);
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }
}
