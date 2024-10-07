using FirstProject.Npc;
using Godot;
using System;

public partial class AccuracyModLineEdit : LineEdit, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }
    private int? _accuracyMod = null;
    private const int DEFAULT_MOD = 0;
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
        if (_basicAttack == null) return;
        _accuracyMod = _accuracyMod ?? _basicAttack.AttackMod;

        _basicAttack.AttackMod = _accuracyMod.Value;
        this.Text = _accuracyMod.ToString();
    }

    public void HandleTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            _accuracyMod = DEFAULT_MOD;
            OnEquipmentUpdated?.Invoke();
            return;
        }
        if (int.TryParse(newText, out var mod))
        {
            _accuracyMod = mod;
            _basicAttack.AttackMod = _accuracyMod.Value;
            OnEquipmentUpdated?.Invoke();
        }
        else
        {
            this.Text = _accuracyMod.ToString();
        }
    }
}
