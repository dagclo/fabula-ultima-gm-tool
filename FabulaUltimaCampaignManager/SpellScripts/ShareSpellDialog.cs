using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;

public partial class ShareSpellDialog : Window
{
    [Signal]
    public delegate void SpellUpdatedEventHandler(SignalWrapper<SpellTemplate> spell);

    public Action OnClose { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    internal void SetSpell(SpellTemplate spellTemplate)
    {
        this.Title = spellTemplate.Name;
        EmitSignal(SignalName.SpellUpdated, new SignalWrapper<SpellTemplate>(spellTemplate));
    }

    public void OnCloseRequested()
    {
        this.Hide();
        OnClose.Invoke();
    }
}
