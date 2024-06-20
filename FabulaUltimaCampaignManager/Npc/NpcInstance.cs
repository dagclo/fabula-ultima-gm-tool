using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstProject.Npc
{
    public partial class NpcInstance : Resource
    {
        public IBeastTemplate Template => new SkilledBeastTemplateWrapper(new BeastTemplate(Model))
        {
            Immutable = true
        };
        public NpcInstance()
        {
            Id = Guid.NewGuid().ToString();            
        }

        [Export]
        public string Id { get; set; }

        private NpcModel _model;
        [Export]
        public NpcModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if(_model != null)
                {
                    _model.Changed -= OnModelChanged;
                }
                _model = value;
                _model.Changed += OnModelChanged;
            }
        }

        private void OnModelChanged()
        {
            EmitChanged();
        }

        private string _instanceName;
        [Export]
        public string InstanceName
        {
            get
            {
                return _instanceName;
            }
            set
            {
                _instanceName = value;
                EmitChanged();
            }
        }



        public IReadOnlyDictionary<string, Affinity> Resistances => this.Template.Resistances.ToDictionary(p => p.Key, p => p.Value.ToAffinity());

        public string GetValueOf(string attributeName)
        {
            string text;
            switch (attributeName)
            {
                case "Name":
                    text = InstanceName;
                    break;
                case "Type":
                    text = Model.Name;
                    break;
                case "PDef":
                    text = Template.Defense.ToString();
                    break;
                case "MDef":
                    text = Template.MagicalDefense.ToString();
                    break;
                default:
                    text = "";
                    break;
            }

            return text;
        }
    }
}
