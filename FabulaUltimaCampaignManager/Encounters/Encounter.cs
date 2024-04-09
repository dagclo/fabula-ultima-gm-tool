using FirstProject.Npc;
using Godot;
using System;

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
