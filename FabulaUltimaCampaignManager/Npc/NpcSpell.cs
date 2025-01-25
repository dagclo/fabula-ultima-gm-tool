using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcSpell : Resource
    {
        public NpcSpell(): this(new SpellTemplate()) { }

        public NpcSpell(SpellTemplate template) 
        {
            SpellTemplate = template;
            _damageType = template.DamageType != null ? new NpcDamageType(template.DamageType) : null;
        }

        public SpellTemplate SpellTemplate { get; set; }

        [Export]
        public string Id
        {
            get
            {
                return SpellTemplate.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    SpellTemplate.Id = guid;
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
                return SpellTemplate.Name;
            }
            set
            {
                SpellTemplate.Name = value;
            }
        }

        [Export]
        public string Attribute1
        {
            get
            {
                return SpellTemplate.Attribute1;
            }
            set
            {
                SpellTemplate.Attribute1 = value;
            }
        }

        [Export]
        public string Attribute2
        {
            get
            {
                return SpellTemplate.Attribute2;
            }
            set
            {
                SpellTemplate.Attribute2 = value;
            }
        }

        [Export]
        public string Description
        {
            get
            {
                return SpellTemplate.Description;
            }
            set
            {
                SpellTemplate.Description = value;
            }
        }

        [Export]
        public string Duration
        {
            get
            {
                return SpellTemplate.Duration;
            }
            set
            {
                SpellTemplate.Duration = value;
            }
        }

        [Export]
        public string Target
        {
            get
            {
                return SpellTemplate.Target;
            }
            set
            {
                SpellTemplate.Target = value;
            }
        }

        [Export]
        public int MagicPointCost
        {
            get
            {
                return SpellTemplate.MagicPointCost;
            }
            set
            {
                SpellTemplate.MagicPointCost = value;
            }
        }

        [Export]
        public bool IsOffensive
        {
            get
            {
                return SpellTemplate.IsOffensive;
            }
            set
            {
                SpellTemplate.IsOffensive = value;
            }
        }

        [Export]
        public int DamageModifier
        {
            get
            {
                return SpellTemplate.DamageModifier ?? 0;
            }
            set
            {
                SpellTemplate.DamageModifier = value;
            }
        }

        private NpcDamageType _damageType;

        [Export]
        public NpcDamageType DamageType
        {
            get
            {
                return _damageType;
            }
            set
            {

                _damageType = value;
                SpellTemplate.DamageType = _damageType.DamageType;
            }
        }
    }
}