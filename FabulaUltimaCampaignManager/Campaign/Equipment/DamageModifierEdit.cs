using FirstProject.Npc;
using Godot;
using System;

public partial class DamageModifierEdit : LineEdit, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }
    private int? _damageMod = null;
    private const int DEFAULT_MOD = 5;
    private NpcBasicAttack _basicAttack;

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

    public void HandleBasicAttackSet(NpcBasicAttack basicAttack)
    {
        _basicAttack = basicAttack;
        _basicAttack = basicAttack;
        if (_basicAttack == null) return;
        _damageMod = _damageMod ?? _basicAttack.DamageMod;

        _basicAttack.DamageMod = _damageMod.Value;
        this.Text = _damageMod.ToString();
    }

    public void HandleTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            _damageMod = DEFAULT_MOD;
            OnEquipmentUpdated?.Invoke();
            return;
        }
        if (int.TryParse(newText, out var mod))
        {
            _damageMod = mod;
            _basicAttack.DamageMod = _damageMod.Value;
            OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _damageMod.ToString();
        }
    }
}
