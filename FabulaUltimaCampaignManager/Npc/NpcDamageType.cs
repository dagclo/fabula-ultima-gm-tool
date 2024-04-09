using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcDamageType : Resource
    {
        public NpcDamageType() : this(new FabulaUltimaNpc.DamageType())
        {

        }

        public NpcDamageType(FabulaUltimaNpc.DamageType damageType)
        {
            DamageType = damageType;
        }

        public FabulaUltimaNpc.DamageType DamageType { get; set; }

        [Export]
        public string Id
        {
            get
            {
                return DamageType.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    DamageType.Id = guid;
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
                return DamageType.Name;
            }
            set
            {
                DamageType.Name = value;
            }
        }
    }
}