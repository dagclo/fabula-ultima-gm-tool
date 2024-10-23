using FabulaUltimaNpc;
using FirstProject.Npc;
using Godot;
using System;

public partial class ShareSpellButton : Button, ISpellReader
{
    private SpellTemplate _spellTemplate;

    [Export]
    public PackedScene ShareSpellDialog { get; set; }

    private ShareSpellDialog _shareSpellDialog { get; set; }
    

    public void Read(SpellTemplate spellTemplate)
    {
        _spellTemplate = spellTemplate;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void OnPressed()
    {
        _shareSpellDialog = ShareSpellDialog.Instantiate<ShareSpellDialog>();
        _shareSpellDialog.OnClose += OnClose;
        AddChild(_shareSpellDialog);
        _shareSpellDialog.SetSpell(_spellTemplate);
        _shareSpellDialog.Show();
        this.Disabled = true;
    }

    private void OnClose()
    {
        _shareSpellDialog.OnClose -= OnClose;
        RemoveChild(_shareSpellDialog);
        _shareSpellDialog.QueueFree();
        _shareSpellDialog = null;
        this.Disabled = false;
    }
}
