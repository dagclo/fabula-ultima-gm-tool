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
            CanBeModified = true,
            CanBeDeleted = false,
        };
        public NpcInstance()
        {
            Id = Guid.NewGuid().ToString();            
        }

        public NpcInstance(NpcInstance npc) : this()
        {
            Model = npc.Model;
            InstanceName = npc.InstanceName;
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

        public string GetStringValueOf(string attributeName)
        {
            string val;
            switch (attributeName)
            {
                case "PDef":
                    val = Template.Defense.ToString();
                    break;
                case "MDef":
                    val = Template.MagicalDefense.ToString();
                    break;
                case nameof(Template.Might):
                    val = Template.Might.ToString();
                    break;
                case nameof(Template.Dexterity):
                    val = Template.Dexterity.ToString();
                    break;
                case nameof(Template.Insight):
                    val = Template.Insight.ToString();
                    break;
                case nameof(Template.WillPower):
                    val = Template.WillPower.ToString();
                    break;
                case nameof(Template.Name):
                    val = Template.Name;
                    break;
                default:
                    val = string.Empty;
                    break;
            }

            return val;
        }
    }
}
