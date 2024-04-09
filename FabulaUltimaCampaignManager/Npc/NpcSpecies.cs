using FabulaUltimaNpc;
using Godot;
using System;

namespace FirstProject.Npc
{
    public partial class NpcSpecies : Resource
    {
        public SpeciesType SpeciesType { get; set; }

        public NpcSpecies() : this(new SpeciesType { Id = Guid.Empty, Name = string.Empty}) 
        {
            
        }

        public NpcSpecies(SpeciesType species) 
        { 
            SpeciesType = species;
        }

        [Export]
        public string Id
        {
            get
            {   
                return SpeciesType.Id.ToString();
            }
            set
            {   
                if (Guid.TryParse(value, out var guid))
                {
                    SpeciesType.Id = guid;
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
                return SpeciesType.Name;
            }
            set
            {
                SpeciesType.Name = value;
            }
        }
    }
}
