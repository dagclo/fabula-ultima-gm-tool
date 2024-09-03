using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;
using System.Text;

public partial class ClipboardButton : Button, INpcEquipmentReader
{
    private NpcEquipment _equipment;

    public Action OnEquipmentUpdated { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        _equipment = equipment;
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        // do nothing
    }

    public void HandlePressed()
    {
        if (_equipment?.Name == null) return;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Name: {_equipment.Name}");
        stringBuilder.AppendLine();
        if(_equipment.Cost != default)
        {
            stringBuilder.AppendLine($"Cost: {_equipment.Cost} z");
        }
        if (!string.IsNullOrWhiteSpace(_equipment.Quality))
        {
            stringBuilder.AppendLine($"Quality: {_equipment.Quality}");
        }
        if(_equipment.Category.Id != null)
        {
            if (_equipment.Category.IsWeapon)
            {
                var accMod = _equipment.BasicAttack.AttackMod == default ? string.Empty : $"+{_equipment.BasicAttack.AttackMod}";
                stringBuilder.AppendLine($"Accuracy:【{_equipment.BasicAttack.Attribute1.ShortenAttribute()}+{_equipment.BasicAttack.Attribute2.ShortenAttribute()}{accMod}】");
                stringBuilder.AppendLine($"Damage:【HR + {_equipment.BasicAttack.DamageMod}】{_equipment.BasicAttack.DamageType.Name}");
                stringBuilder.AppendLine($"Handedness: {_equipment.NumHands}");
                stringBuilder.AppendLine($"Ranged: {_equipment.Category.IsRanged}");
            }
        }
                
        DisplayServer.ClipboardSet(stringBuilder.ToString());
    }
}
