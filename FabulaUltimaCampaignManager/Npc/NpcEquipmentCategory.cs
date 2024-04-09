using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcEquipmentCategory : Resource
    {
        public NpcEquipmentCategory() : this(new EquipmentCategory())
        {

        }

        public NpcEquipmentCategory(EquipmentCategory equipmentCategory)
        {
            EquipmentCategory = equipmentCategory;
        }

        public EquipmentCategory EquipmentCategory { get; set; }

        [Export]
        public string Id
        {
            get
            {
                return EquipmentCategory.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    EquipmentCategory.Id = guid;
                }
                else
                {
                    throw new ArgumentException($"{nameof(Id)} must be {nameof(Guid)}.");
                }
            }
        }

        [Export]
        public string Name
        {
            get
            {
                return EquipmentCategory.Name;
            }
            set
            {
                EquipmentCategory.Name = value;
            }
        }

        [Export]
        public bool IsWeapon
        {
            get
            {
                return EquipmentCategory.IsWeapon;
            }
            set
            {
                EquipmentCategory.IsWeapon = value;
            }
        }

        [Export]
        public bool IsArmor
        {
            get
            {
                return EquipmentCategory.IsArmor;
            }
            set
            {
                EquipmentCategory.IsArmor = value;
            }
        }

        [Export]
        public bool IsRanged
        {
            get
            {
                return EquipmentCategory.IsRanged;
            }
            set
            {
                EquipmentCategory.IsRanged = value;
            }
        }
    }
}