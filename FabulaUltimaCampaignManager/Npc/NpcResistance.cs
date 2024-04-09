using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcResistance : Resource
    {
        public NpcResistance(): this(new BeastResistance()) { }

        public NpcResistance(BeastResistance beastResistance) 
        {
            BeastResistance = beastResistance;
        }

        public BeastResistance BeastResistance { get; set; }

        [Export]
        public string DamageTypeId
        {
            get
            {
                return BeastResistance.DamageTypeId.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    BeastResistance.DamageTypeId = guid;
                }
                else
                {
                    throw new ArgumentException($"{nameof(DamageTypeId)} must be {nameof(Guid)}.");
                }
            }
        }


        [Export]
        public string DamageType
        {
            get
            {
                return BeastResistance.DamageType;
            }
            set
            {
                BeastResistance.DamageType = value;
            }
        }

        [Export]
        public string AffinityId
        {
            get
            {
                return BeastResistance.AffinityId.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    BeastResistance.AffinityId = guid;
                }
                else
                {
                    throw new ArgumentException($"{nameof(AffinityId)} must be {nameof(Guid)}.");
                }
            }
        }

        [Export]
        public string Affinity
        {
            get
            {
                return BeastResistance.Affinity;
            }
            set
            {
                BeastResistance.Affinity = value;
            }
        }
    }
}