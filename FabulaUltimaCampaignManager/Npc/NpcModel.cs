using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstProject.Npc
{
    public partial class NpcModel : Resource, IBeast
    {
        public NpcModel() 
        { 
            
        }

        public NpcModel(IBeast otherModel) 
        {
            this.Id = otherModel.Id;
            this.Name = otherModel.Name;
            this.Description = otherModel.Description;
            this.Level = otherModel.Level;
            this.Might = otherModel.Might;
            this.Dexterity = otherModel.Dexterity;
            this.Insight = otherModel.Insight;
            this.WillPower = otherModel.WillPower;
            this.Traits = otherModel.Traits;
            this.NpcSpecies = new NpcSpecies(otherModel.Species);
            this.ImageFile = otherModel.ImageFile;
            this.NpcResistances = new Godot.Collections.Dictionary<string, NpcResistance>(otherModel.Resistances.ToDictionary(p => p.Key, p => new NpcResistance(p.Value)));
            this.NpcAttacks = new Godot.Collections.Array<NpcBasicAttack>(otherModel.BasicAttacks.Select(a => new NpcBasicAttack(a)));
            this.NpcSpells = new Godot.Collections.Array<NpcSpell>(otherModel.Spells.Select(s => new NpcSpell(s)));
            this.NpcEquipment = new Godot.Collections.Array<NpcEquipment>(otherModel.Equipment.Select(e => new NpcEquipment(e)));
            this.NpcSkills = new Godot.Collections.Array<NpcSkill>(otherModel.Skills.Select(s => new NpcSkill(s))); 
            this.NpcActions = new Godot.Collections.Array<NpcAction>(otherModel.Actions.Select(a => new NpcAction(a)));            
        }

        private FabulaUltimaNpc.Rank _instanceRank = FabulaUltimaNpc.Rank.Soldier;
        [Export]
        public FabulaUltimaNpc.Rank Rank
        {
            get
            {
                return _instanceRank;
            }
            set
            {
                _instanceRank = value;
                EmitChanged();
            }
        }

        private string _id;
        [Export]
        public string IdString {
            get
            {   
                return _id;
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    _id = guid.ToString();
                }
                else
                {
                    throw new ArgumentException($"{nameof(IdString)} must be {nameof(Guid)}."); 
                }
            }
        }

        public Guid Id 
        {
            get
            {
                return Guid.Parse( IdString );
            }
            set
            {
                IdString = value.ToString();
            }
        }

        [Export]
        public string Name { get; set; }

        [Export]
        public string Description { get; set; }

        public int _level;
        [Export]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                EmitChanged();
            }
        }

        [Export]
        public string Traits { get; set; }

        [Export]
        public NpcSpecies NpcSpecies { get; set; }

        public SpeciesType Species
        {
            get
            {
                return NpcSpecies.SpeciesType;
            }
            set
            {
                NpcSpecies.SpeciesType = value;
            }
        }

        [Export]
        public int InsightSides { get; set; }
        public Die Insight 
        {
            get
            {
                return new Die(InsightSides);
            }
            set
            {
                InsightSides = value.Sides;
            }
        }
        
        [Export]
        public int DexteritySides { get; set; }
        public Die Dexterity
        {
            get
            {
                return new Die(DexteritySides);
            }
            set
            {
                DexteritySides = value.Sides;
            }
        }
        
        [Export]
        public int MightSides { get; set; }
        public Die Might
        {
            get
            {
                return new Die(MightSides);
            }
            set
            {
                MightSides = value.Sides;
            }
        }
        
        [Export]
        public int WillPowerSides { get; set; }
        public Die WillPower
        {
            get
            {
                return new Die(WillPowerSides);
            }
            set
            {
                WillPowerSides = value.Sides;
            }
        }

        [Export]
        public string ImageFile { get; set; }

        [Export]
        public Godot.Collections.Dictionary<string, NpcResistance> NpcResistances { get; set; }

        IDictionary<string, BeastResistance> IBeast.Resistances => NpcResistances.ToDictionary(r => r.Key, r => r.Value.BeastResistance);

        [Export]
        public Godot.Collections.Array<NpcBasicAttack> NpcAttacks { get; set; }
        ICollection<BasicAttackTemplate> IBeast.BasicAttacks => NpcAttacks.Select(a => a.BasicAttackTemplate).ToList();


        [Export]
        public Godot.Collections.Array<NpcSpell> NpcSpells { get; set; }
        ICollection<SpellTemplate> IBeast.Spells => NpcSpells.Select(s => s.SpellTemplate).ToList();

        [Export]
        public Godot.Collections.Array<NpcEquipment> NpcEquipment { get; set; }
        ICollection<EquipmentTemplate> IBeast.Equipment => NpcEquipment.Select(e => e.EquipmentTemplate).ToList();

        [Export]
        public Godot.Collections.Array<NpcSkill> NpcSkills { get; set; }

        ICollection<SkillTemplate> IBeast.Skills => NpcSkills.Select(s => s.SkillTemplate).Concat(RankSkills?.Select(s => s.SkillTemplate) ?? new SkillTemplate[0]).ToList();

        [Export]
        public Godot.Collections.Array<NpcAction> NpcActions { get; set; }

        ICollection<ActionTemplate> IBeast.Actions => NpcActions.Select(s => s.ActionTemplate).ToList();

        
        private Godot.Collections.Array<NpcSkill> _rankSkills;
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
                EmitChanged();
            }
        }
    }
}