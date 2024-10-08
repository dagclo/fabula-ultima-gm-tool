using FirstProject.Npc;
using Godot;
using System;

public partial class InitiativeLineEdit : LineEdit, INpcEquipmentReader
{
    private NpcEquipmentModifiers _modifiers;
    private int? _initiative;

    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
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
        _initiative = _initiative ?? _modifiers.InitiativeModifier;

        _modifiers.InitiativeModifier = _initiative.Value;
        this.Text = _initiative.ToString();
    }

    public void HandleTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            _initiative = 0;
            OnEquipmentUpdated?.Invoke();
            return;
        }
        if (int.TryParse(newText, out var mod))
        {
            _initiative = mod;
            _modifiers.InitiativeModifier = _initiative.Value;
            OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _initiative.ToString();
        }
    }
}
