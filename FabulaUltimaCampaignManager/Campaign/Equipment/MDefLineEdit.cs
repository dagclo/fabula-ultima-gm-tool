using FirstProject.Npc;
using Godot;
using System;

public partial class MDefLineEdit : LineEdit, INpcEquipmentReader
{
    private NpcEquipmentModifiers _modifiers;
    private int? _mDefMod;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        // do nothing
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        // do nothing
    }

    public void HandleModifiersSet(NpcEquipmentModifiers modifiers)
    {
        _modifiers = modifiers;
        if (_modifiers == null) return;
        _mDefMod = _mDefMod ?? _modifiers.MagicDefenseModifier;

        _modifiers.MagicDefenseModifier = _mDefMod.Value;
        this.Text = _mDefMod.ToString();
    }

    public void HandleTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            _mDefMod = 0;
            OnEquipmentUpdated?.Invoke();
            return;
        }
        if (int.TryParse(newText, out var mod))
        {
            _mDefMod = mod;
            _modifiers.MagicDefenseModifier = _mDefMod.Value;
            OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _mDefMod.ToString();
        }
    }
}
