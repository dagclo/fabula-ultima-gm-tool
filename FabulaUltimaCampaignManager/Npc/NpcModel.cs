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

        IReadOnlyDictionary<string, BeastResistance> IBeast.Resistances => NpcResistances.ToDictionary(r => r.Key, r => r.Value.BeastResistance);

        [Export]
        public Godot.Collections.Array<NpcBasicAttack> NpcAttacks { get; set; }
        IReadOnlyCollection<BasicAttackTemplate> IBeast.BasicAttacks => NpcAttacks.Select(a => a.BasicAttackTemplate).ToList();

        public void RemoveBasicAttack(BasicAttackTemplate basicAttack)
        {
            var equipmentId = basicAttack.Id;
            int? targetIndex = null;
            foreach (var (equipment, index) in NpcAttacks.Select((s, i) => (s, i)))
            {
                if (Guid.Parse(equipment.Id) == basicAttack.Id)
                {
                    targetIndex = index;
                    break;
                }
            }

            if (targetIndex != null)
            {
                NpcAttacks.RemoveAt(targetIndex.Value);
                return;
            }
        }

        public void AddBasicAttack(BasicAttackTemplate basicAttack)
        {
            NpcAttacks.Add(new NpcBasicAttack(basicAttack));
        }


        [Export]
        public Godot.Collections.Array<NpcSpell> NpcSpells { get; set; }
        IReadOnlyCollection<SpellTemplate> IBeast.Spells => NpcSpells.Select(s => s.SpellTemplate).ToList();

        public void RemoveSpell(SpellTemplate targetSpell)
        {
            var equipmentId = targetSpell.Id;
            int? targetIndex = null;
            foreach (var (spell, index) in NpcSpells.Select((s, i) => (s, i)))
            {
                if (Guid.Parse(spell.Id) == targetSpell.Id)
                {
                    targetIndex = index;
                    break;
                }
            }

            if (targetIndex != null)
            {
                NpcSpells.RemoveAt(targetIndex.Value);
                return;
            }
        }

        public void AddSpell(SpellTemplate spell)
        {
            NpcSpells.Add(new NpcSpell(spell));
        }

        [Export]
        public Godot.Collections.Array<NpcEquipment> NpcEquipment { get; set; }
        public void RemoveEquipment(EquipmentTemplate targetEquipment)
        {
            var equipmentId = targetEquipment.Id;
            int? targetIndex = null;
            foreach (var (equipment, index) in NpcEquipment.Select((s, i) => (s, i)))
            {
                if (Guid.Parse(equipment.Id) == targetEquipment.Id)
                {
                    targetIndex = index;
                    break;
                }
            }

            if (targetIndex != null)
            {
                NpcEquipment.RemoveAt(targetIndex.Value);
                return;
            }         
        }

        public void AddEquipment(EquipmentTemplate equipment)
        {
            NpcEquipment.Add(new NpcEquipment(equipment));
        }

        public bool HasEquipment(EquipmentTemplate equipment) => NpcEquipment.Any(e => Guid.Parse(e.Id) == equipment.Id);
        

        IReadOnlyCollection<EquipmentTemplate> IBeast.Equipment => NpcEquipment.Select(e => e.EquipmentTemplate).ToList();

        [Export]
        public Godot.Collections.Array<NpcSkill> NpcSkills { get; set; }

        public void RemoveSkill(SkillTemplate targetSkill)
        {
            var skillId = targetSkill.Id;
            int? baseSkillIndex = null;
            foreach(var (skill, index) in NpcSkills.Select((s, i) => (s, i)))
            {
                if(Guid.Parse(skill.Id) == targetSkill.Id)
                {
                    baseSkillIndex = index;
                    break;
                }
            }

            if(baseSkillIndex != null)
            {                
                NpcSkills.RemoveAt(baseSkillIndex.Value);
                return;
            }

            int? rankSkillIndex = null;
            foreach (var (skill, index) in RankSkills.Select((s, i) => (s, i)))
            {
                if (Guid.Parse(skill.Id) == targetSkill.Id)
                {
                    rankSkillIndex = index;
                    break;
                }
            }

            if (rankSkillIndex != null)
            {
                RankSkills.RemoveAt(rankSkillIndex.Value);
                return;
            }
        }

        public void AddSkill(SkillTemplate skill)
        {
            NpcSkills.Add(new NpcSkill(skill));
        }

        IReadOnlyCollection<SkillTemplate> IBeast.Skills => NpcSkills.Select(s => s.SkillTemplate).Concat(RankSkills?.Select(s => s.SkillTemplate) ?? new SkillTemplate[0]).ToList();

        [Export]
        public Godot.Collections.Array<NpcAction> NpcActions { get; set; }

        IReadOnlyCollection<ActionTemplate> IBeast.Actions => NpcActions.Select(s => s.ActionTemplate).ToList();

        public void RemoveAction(ActionTemplate basicAttack)
        {
            var equipmentId = basicAttack.Id;
            int? targetIndex = null;
            foreach (var (action, index) in NpcActions.Select((s, i) => (s, i)))
            {
                if (Guid.Parse(action.Id) == basicAttack.Id)
                {
                    targetIndex = index;
                    break;
                }
            }

            if (targetIndex != null)
            {
                NpcActions.RemoveAt(targetIndex.Value);
                return;
            }
        }

        public void AddAction(ActionTemplate action)
        {
            NpcActions.Add(new NpcAction(action));
        }


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