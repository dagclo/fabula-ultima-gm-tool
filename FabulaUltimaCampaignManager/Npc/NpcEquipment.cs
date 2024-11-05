using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcEquipment : Resource
    {
        public NpcEquipment(): this(new EquipmentTemplate())
        {

        }

        public NpcEquipment(EquipmentTemplate template)
        {
            EquipmentTemplate = template.Clone();
            Category = new NpcEquipmentCategory(EquipmentTemplate.Category);
            if(EquipmentTemplate.BasicAttack != null) BasicAttack = new NpcBasicAttack(EquipmentTemplate.BasicAttack);
            if(EquipmentTemplate.StatsModifier != null)  Modifiers = new NpcEquipmentModifiers(EquipmentTemplate.StatsModifier);
        }

        public EquipmentTemplate EquipmentTemplate { get; }

        [Export]
        public string Id
        {
            get
            {
                return EquipmentTemplate.Id?.ToString();
            }
            set
            {
                if(value == null)
                {
                    EquipmentTemplate.Id = null;
                }
                else if (Guid.TryParse(value, out var guid))
                {
                    EquipmentTemplate.Id = guid;
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
                return EquipmentTemplate.Name;
            }
            set
            {
                EquipmentTemplate.Name = value;
            }
        }

        [Export]
        public int Cost
        {
            get
            {
                return EquipmentTemplate.Cost;
            }
            set
            {
                EquipmentTemplate.Cost = value;
            }
        }

        [Export]
        public string Quality
        {
            get
            {
                return EquipmentTemplate.Quality;
            }
            set
            {
                EquipmentTemplate.Quality = value;
            }
        }

        [Export]
        public bool IsMartial
        {
            get
            {
                return EquipmentTemplate.IsMartial;
            }
            set
            {
                EquipmentTemplate.IsMartial = value;
            }
        }

        [Export]
        public int NumHands
        {
            get
            {
                return EquipmentTemplate.NumHands ?? -1;
            }
            set
            {
                EquipmentTemplate.NumHands = value >= 0 ? value : null;
            }
        }

        private NpcEquipmentCategory _category;

        [Export]
        public NpcEquipmentCategory Category
        {
            get
            {
                return _category;                
            }
            set
            {
                _category = value;
                EquipmentTemplate.Category = _category.EquipmentCategory;
            }
        }

        private NpcBasicAttack _basicAttack;

        [Export]
        public NpcBasicAttack BasicAttack
        {
            get
            {
                return _basicAttack;                
            }
            set
            {
                _basicAttack = value;
                EquipmentTemplate.BasicAttack = _basicAttack?.BasicAttackTemplate;
            }
        }

        private NpcEquipmentModifiers _modifiers;
        [Export]
        public NpcEquipmentModifiers Modifiers
        {
            get
            {
                return _modifiers;
            }
            set
            {
                _modifiers = value;
                EquipmentTemplate.StatsModifier = _modifiers.StatsModifications;
            }
        }
    }
}