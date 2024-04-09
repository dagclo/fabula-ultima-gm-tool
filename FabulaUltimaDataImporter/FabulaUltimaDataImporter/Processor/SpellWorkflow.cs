using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FabulaUltimaDataImporter.Processor
{
    internal class SpellWorkflow : IWorkflow
    {
        private readonly Instance _database;
        private readonly UserIOWrapper _userIOWrapper;
        private readonly IWorkflowFactory _workflowFactory;

        public string GetName => nameof(SpellWorkflow);

        public WorkFlowKind Kind => WorkFlowKind.SPELL;

        public SpellEntry? InitialSpell { private get; set; }

        public SpellWorkflow(Instance db, UserIOWrapper userIOWrapper, IWorkflowFactory workflowFactory)
        {
            _database = db;
            _userIOWrapper = userIOWrapper;
            _workflowFactory = workflowFactory;
        }

        public IEnumerable<IWorkflow> Run()
        {
            SpellEntry currentSpell;
            if(InitialSpell == null)
            {
                _userIOWrapper.WriteLine("Creating new spell");
                var spellName = _userIOWrapper.GetValidString("Spell Name");
                currentSpell = new SpellEntry
                {
                    Id = Guid.NewGuid(),
                    Name = spellName
                };
            }
            else
            {
                _userIOWrapper.WriteLine($"Creating Spell {InitialSpell.Name}");
                currentSpell = InitialSpell;
            }

            currentSpell.Duration = GetDuration();
            currentSpell.Target = GetTarget();
            currentSpell.MagicPointCost = (int)_userIOWrapper.GetUnsignedInt("Magic Point Cost", 1);
            currentSpell.Description = _userIOWrapper.GetValidString("Description");
            currentSpell.Attribute1 = _userIOWrapper.GetAttribute(1);
            currentSpell.Attribute2 = _userIOWrapper.GetAttribute(2);
            currentSpell.IsOffensive = _userIOWrapper.GetBoolean($"Is this spell offensive?", "yes", "no");

            _database.CreateSpell(currentSpell);
            _userIOWrapper.WriteLine("Spell created");
            yield break;
        }

        private string GetTarget()
        {
            _userIOWrapper.WriteLine($"Define spell's target. Some suggestions are {string.Join(", ", "Self", "One creature", "The shoes" )}");
            return _userIOWrapper.GetValidString("Target");
        }

        private const string INSTANTANEOUS_SHORT_FORM = "INSTANT";
        private const string INSTANTANEOUS_LONG_FORM = "INSTANTANEOUS";
        private static IReadOnlySet<string> DURATION_VALUES = new HashSet<string>()
        {
           INSTANTANEOUS_LONG_FORM,
           INSTANTANEOUS_SHORT_FORM,
           "SCENE"
        };

        private string GetDuration()
        {
            (bool verified, string error) OnlyAllowDurationValues(string arg)
            {
                var isGoodValue = true;
                var errorMessage = string.Empty;
                if (!DURATION_VALUES.Contains(arg, StringComparer.InvariantCultureIgnoreCase))
                {
                    errorMessage = "Please choose a valid value";
                    isGoodValue = false;
                }
                return (isGoodValue, errorMessage);
            };

            _userIOWrapper.WriteLine($"Define a duration: {string.Join(", ", DURATION_VALUES)})");
            var duration = _userIOWrapper.GetValidString("Duration", additionalVerification: OnlyAllowDurationValues);
            if(string.Equals(duration, INSTANTANEOUS_SHORT_FORM))
            {
                duration = INSTANTANEOUS_LONG_FORM;
            }
            return duration;
        }
    }
}
