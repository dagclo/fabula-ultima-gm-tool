using FirstProject.Npc;
using Godot;
using System;

public partial class PDefLineEdit : LineEdit, INpcEquipmentReader
{
    private NpcEquipmentModifiers _modifiers;
    private int? _pDefMod;

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
        _pDefMod = _pDefMod ?? _modifiers.DefenseModifier;

        _modifiers.DefenseModifier = _pDefMod.Value;
        this.Text = _pDefMod.ToString();
    }

    public void HandleTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            _pDefMod = 0;
            OnEquipmentUpdated?.Invoke();
            return;
        }
        if (int.TryParse(newText, out var mod))
        {
            _pDefMod = mod;
            _modifiers.DefenseModifier = _pDefMod.Value;
            OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _pDefMod.ToString();
        }
    }
}
