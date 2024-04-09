using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcAction : Resource
    {
        public NpcAction() : this(new ActionTemplate()) { }

        public NpcAction(ActionTemplate template)
        {
            ActionTemplate = template;
        }

        public ActionTemplate ActionTemplate { get; set; }

        [Export]
        public string Id
        {
            get
            {
                return ActionTemplate.Id.ToString();
            }
            set
            {
                if (Guid.TryParse(value, out var guid))
                {
                    ActionTemplate.Id = guid;
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
                return ActionTemplate.Name;
            }
            set
            {
                ActionTemplate.Name = value;
            }
        }

        [Export]
        public string Effect
        {
            get
            {
                return ActionTemplate.Effect;
            }
            set
            {
                ActionTemplate.Effect = value;
            }
        }       
    }
}