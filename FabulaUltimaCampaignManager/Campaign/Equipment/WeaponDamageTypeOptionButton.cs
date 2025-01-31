using FabulaUltimaSkillLibrary;
using FirstProject.Npc;
using System;

public partial class WeaponDamageTypeOptionButton : ChooseDamageTypeOptions, INpcEquipmentReader
{
    private NpcBasicAttack _basicAttack;
    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
        this.Selected = 7;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleSelected(int index)
    {
        var damageTypeName = DamageTypes[GetItemId(index)];
        _basicAttack.DamageType = new NpcDamageType(new FabulaUltimaNpc.DamageType
        {
            Id = DamageConstants.DamageTypeMap[damageTypeName],
            Name = damageTypeName,
        });
        OnEquipmentUpdated?.Invoke();
    }

    public void HandleBasicAttackSet(NpcBasicAttack basicAttack)
    {
        _basicAttack = basicAttack;
        if (_basicAttack == null) return;
        var damageTypeName = DamageTypes[GetItemId(Selected)];
        _basicAttack.DamageType = new NpcDamageType( new FabulaUltimaNpc.DamageType
        {
            Id = DamageConstants.DamageTypeMap[damageTypeName],
            Name = damageTypeName,
        });
    }

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        // do nothing   
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {
        // do nothing       
    }
}
