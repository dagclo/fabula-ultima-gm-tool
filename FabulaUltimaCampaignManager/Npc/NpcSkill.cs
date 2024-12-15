using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using Godot;
using Godot.Collections;
using System;
using System.Linq;

namespace FirstProject.Npc
{
    /// <summary>
    /// Wrapper for GodotSerialization
    /// </summary>
    public partial class NpcSkill : Resource
    {
        public SkillTemplate SkillTemplate { get; set; }

        public NpcSkill() :this(new SkillTemplate(Guid.Empty)) { }

        public NpcSkill(SkillTemplate skill) 
        {
            SkillTemplate = skill;
        }

        [Export]
        public string Id
        {
            get
            {
                return SkillTemplate.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    SkillTemplate.Id = guid;
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
                return SkillTemplate.Name;
            }
            set
            {
                SkillTemplate.Name = value;
            }
        }

        [Export]
        public string TargetType
        {
            get
            {
                return SkillTemplate.TargetType?.ToString();
            }
            set
            {
                SkillTemplate.TargetType = Type.GetType(value);
            }
        }

        [Export]
        public string Text
        {
            get
            {
                return SkillTemplate.Text;
            }
            set
            {
                SkillTemplate.Text = value;
            }
        }

        [Export]
        public bool IsSpecialRule
        {
            get
            {
                return SkillTemplate.IsSpecialRule;
            }
            set
            {
                SkillTemplate.IsSpecialRule = value;
            }
        }

        /// <summary>
        /// Warning: using this method won't allow for modification of members
        /// </summary>
        [Export]        
        public Godot.Collections.Array<string> Keywords
        {
            get
            {   
                return new Godot.Collections.Array<string>(SkillTemplate.Keywords?.ToArray() ?? new string[0]);
            }
            set
            {
                SkillTemplate.Keywords = value.ToHashSet();
            }
        }


        // <summary>
        /// Warning: using this method won't allow for modification of members
        /// </summary>
        [Export]
        public Godot.Collections.Dictionary<string, string> SkillAttributes
        {
            get
            {
                return new Godot.Collections.Dictionary<string, string>(SkillTemplate?.OtherAttributes?.DataDictionary ?? new Dictionary<string, string>());
            }
            set
            {
                SkillTemplate.OtherAttributes = new SkillAttributeCollection(value);
            }
        }
    }
}
