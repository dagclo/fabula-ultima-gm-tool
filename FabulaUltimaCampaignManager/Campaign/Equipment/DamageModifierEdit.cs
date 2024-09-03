using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;

public partial class DamageModifierEdit : LineEdit, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }
    private int _damageMod = 5;
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
        if (_basicAttack != null)
        {
            _basicAttack.DamageMod = _damageMod;
        }
    }

    public void HandleTextChanged(string newText)
    {
        if(int.TryParse(newText, out var mod))
        {
            _damageMod = mod;
            _basicAttack.DamageMod = _damageMod;
        }
        else
        {
            this.Text = _damageMod.ToString();
        }
    }
}
