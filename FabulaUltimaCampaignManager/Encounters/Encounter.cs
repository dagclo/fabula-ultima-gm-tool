using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

namespace FirstProject.Encounters
{

    public partial class Encounter : Resource
    {
        private string _name;

        [Export]
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.EmitChanged();                
            }
        }

        [Export]
        public Godot.Collections.Array<NpcInstance> NpcCollection { get; set; }

        public void AddNpc(NpcInstance npc)
        {
            NpcCollection.Add(npc);
            EmitChanged();
        }

        public void RemoveNpc(NpcInstance npc)
        {
            NpcCollection.Remove(npc);
            EmitChanged();
        }

        internal bool HasTag(Tag tag)
        {
            return Tags?.Any(t => t.Name == tag.Name) ?? false;
        }

        internal void AddTags(params Tag[] tags)
        {
            Tags = Tags ?? new Godot.Collections.Array<Tag>();
            Tags.AddRange(tags);
        }

        internal void RemoveTag(Tag tag)
        {
            if (!HasTag(tag)) return;
            int? foundIndex = null;
            foreach((Tag candidate, int index) in Tags.Select((t, i) => (t, i)))
            {
                if(candidate.Name == tag.Name)
                {
                    foundIndex = index;
                    break;
                }
            }
            if (foundIndex == null) return;
            Tags.RemoveAt(foundIndex.Value);
        }

        [Export]
        public string Id { get; set; }

        private Background _background;
        [Export]
        public Background Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                this.EmitChanged();
            }
        }

        [Export]
        public Godot.Collections.Array<Tag> Tags { get; set; }

        public InitiativeSeed InitiativeSeed { get; set; }

        public Encounter() :this("", Guid.Empty.ToString(), new Godot.Collections.Array<NpcInstance>()) { }

        public Encounter(string name, string id, Godot.Collections.Array<NpcInstance> npcs) 
        {
            Name = name;
            Id = id;
            NpcCollection = npcs;
        }
    }
}
