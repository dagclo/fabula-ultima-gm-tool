using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;
using FabulaUltimaSkillLibrary;

namespace FabulaUltimaDataImporter.Processor
{
    internal class ActionWorkflow : IWorkflow
    {
        private readonly Instance _database;
        private readonly UserIOWrapper _userIOWrapper;

        public string GetName => nameof(ActionWorkflow);
        public ActionEntry? InitialAction { private get; set; }

        public WorkFlowKind Kind => WorkFlowKind.ACTION;

        public ActionWorkflow(Instance db, UserIOWrapper userIOWrapper)
        {
            _database = db;
            _userIOWrapper = userIOWrapper;            
        }

        public IEnumerable<IWorkflow> Run()
        {
            ActionEntry currentAction;
            if (InitialAction == null)
            {
                _userIOWrapper.WriteLine("Creating new Action");
                var spellName = _userIOWrapper.GetValidString("Action name");
                currentAction = new ActionEntry
                {
                    Id = Guid.NewGuid(),
                    Name = spellName
                };
            }
            else
            {
                _userIOWrapper.WriteLine($"Creating Action {InitialAction.Name}");
                currentAction = InitialAction;
            }

            currentAction.Effect = _userIOWrapper.GetValidString("Effect");

            _database.CreateAction(currentAction);
            _userIOWrapper.WriteLine("Action created");
            yield break;
        }
    }
}
