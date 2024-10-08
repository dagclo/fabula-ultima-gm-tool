using FirstProject.Npc;
using Godot;
using System;

public partial class PDefOverrideCheckButton : CheckButton, INpcEquipmentReader
{
    private NpcEquipmentModifiers _modifiers;
    private bool? _defOverride;

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
        _defOverride = _defOverride ?? _modifiers.DefenseOverrides;
        _modifiers.DefenseOverrides = _defOverride.Value;
        this.SetPressedNoSignal(_modifiers.DefenseOverrides);
    }

    public void HandleToggle(bool toggle)
    {
        _defOverride = toggle;
        _modifiers.DefenseOverrides = _defOverride.Value;
        OnEquipmentUpdated?.Invoke();
    }
}
