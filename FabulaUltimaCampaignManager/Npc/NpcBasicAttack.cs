using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstProject.Npc
{
    public partial class NpcBasicAttack : Resource
    {
        public NpcBasicAttack() : this(new BasicAttackTemplate() { DamageType = new FabulaUltimaNpc.DamageType()})
        {

        }

        public NpcBasicAttack(BasicAttackTemplate basicAttackTemplate)
        {
            BasicAttackTemplate = basicAttackTemplate;
            // this have to be set here because Godot does not like return new instances in its properties
            _damageType = new NpcDamageType(BasicAttackTemplate.DamageType);
            IEnumerable<NpcSkill> skills = BasicAttackTemplate.AttackSkills?.Select(s => new NpcSkill(s)) ?? new NpcSkill[0];
            _skills = new Godot.Collections.Array<NpcSkill>(skills);
        }

        public BasicAttackTemplate BasicAttackTemplate { get; set; }

        [Export]
        public string Id
        {
            get
            {
                return BasicAttackTemplate.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    BasicAttackTemplate.Id = guid;
                }
                else
                {
                    throw new ArgumentException($"{nameof(Id)} must be {nameof(Guid)}.");
                }
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
                BasicAttackTemplate.DamageType = _damageType.DamageType;
            }
        }

        [Export]
        public string Name
        {
            get
            {
                return BasicAttackTemplate.Name;
            }
            set
            {
                BasicAttackTemplate.Name = value;
            }
        }

        [Export]
        public string Attribute1
        {
            get
            {
                return BasicAttackTemplate.Attribute1;
            }
            set
            {
                BasicAttackTemplate.Attribute1 = value;
            }
        }

        [Export]
        public string Attribute2
        {
            get
            {
                return BasicAttackTemplate.Attribute2;
            }
            set
            {
                BasicAttackTemplate.Attribute2 = value;
            }
        }

        [Export]
        public int AttackMod
        {
            get
            {
                return BasicAttackTemplate.AccuracyMod;
            }
            set
            {
                BasicAttackTemplate.AccuracyMod = value;
            }
        }

        [Export]
        public int DamageMod
        {
            get
            {
                return BasicAttackTemplate.DamageMod;
            }
            set
            {
                BasicAttackTemplate.DamageMod = value;
            }
        }


        [Export]
        public bool IsRanged
        {
            get
            {
                return BasicAttackTemplate.IsRanged;
            }
            set
            {
                BasicAttackTemplate.IsRanged = value;
            }
        }

        private Godot.Collections.Array<NpcSkill> _skills;
        /// <summary>
        /// WARNING: using this to add/delete skills will not be saved
        /// </summary>
        [Export]        
        public Godot.Collections.Array<NpcSkill> NpcAttackSkills 
        {
            get
            {
                return _skills;
            }
            set
            {
                _skills = value;
                BasicAttackTemplate.AttackSkills = AllSkills.Select(s => s.SkillTemplate).ToList();
            }
        }

        private IEnumerable<NpcSkill> AllSkills => _skills.Concat(_rankSkills);

        private Godot.Collections.Array<NpcSkill> _rankSkills = new Godot.Collections.Array<NpcSkill>();
        [Export]
        public Godot.Collections.Array<NpcSkill> RankSkills
        {
            get
            {
                return _rankSkills;
            }
            set
            {
                _rankSkills = value;
                BasicAttackTemplate.AttackSkills = AllSkills.Select(s => s.SkillTemplate).ToList();
            }
        }
    }
}