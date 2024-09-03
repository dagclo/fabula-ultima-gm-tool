using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class WeaponAttributeOptionButton : OptionButton, INpcEquipmentReader
{
    private NpcEquipment _equipment;
    private NpcBasicAttack _basicAttack;

    [Export]
    public int Index { get; set; }

    [Export]
    public Godot.Collections.Array<string> Attributes { get; set; } = new Godot.Collections.Array<string>(AttributeExtensions.ShortAttributeNames);
    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach((var val, int index) in Attributes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(-1);
        _basicAttack = new NpcBasicAttack()
        {
            Id = Guid.NewGuid().ToString()
        };
    }

    public void OnSelected(int index)
    {
        var attrName = Attributes[GetItemId(index)];
        var longAttributeName = attrName.LengthenAttributeName();
        switch (Index)
        {
            case 0:
                _equipment.BasicAttack.Attribute1 = longAttributeName;
                break;
            case 1:
                _equipment.BasicAttack.Attribute2 = longAttributeName;
                break;
            default:
                throw new ArgumentOutOfRangeException($"{Index} out of range");
        }
    }

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        _equipment = equipment;        
    }

    public void HandleEquipmentChanged(NpcEquipment equipment)
    {        
        if (_equipment.Category.IsWeapon)
        {
            _equipment.BasicAttack = _basicAttack;
            string attributeToCheck;
            switch (Index)
            {
                case 0:
                    attributeToCheck = _basicAttack.Attribute1;
                    break;
                case 1:
                    attributeToCheck = _basicAttack.Attribute2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Index} out of range");
            }
            if (attributeToCheck == null) return;
            var id = Attributes.IndexOf(attributeToCheck.ShortenAttribute());
            this.Selected = GetItemIndex(id);
        }
        else
        {
            if(_equipment.BasicAttack != null) _equipment.BasicAttack = null;
        }
    }
}
