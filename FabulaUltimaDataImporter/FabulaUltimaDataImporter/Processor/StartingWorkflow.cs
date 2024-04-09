using FabulaUltimaDatabase;
using FabulaUltimaDataImporter.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaDataImporter.Processor
{
    public class StartingWorkflow : IWorkflow
    {
        private readonly UserIOWrapper _userIOWrapper;
        private readonly IWorkflowFactory _workflowFactory;

        public StartingWorkflow(UserIOWrapper userIOWrapper, IWorkflowFactory workflowFactory)
        {   
            _userIOWrapper = userIOWrapper;
            _workflowFactory = workflowFactory;
        }

        public string GetName => nameof(StartingWorkflow);

        public WorkFlowKind Kind => WorkFlowKind.INITIAL;

        public IEnumerable<IWorkflow> Run()
        {
            var workflowList = _workflowFactory
                .GetAllAvailableWorkFlows()
                .Where(k => k != WorkFlowKind.INITIAL)
                .OrderBy(k => k)
                .ToArray();

            _userIOWrapper.WriteLines(
                (new string[] { "select workflow" }).Concat(workflowList.Select((w, i) => $"    {i}. {w}")).ToArray()
            );
            uint? selection = _userIOWrapper.GetUnsignedInt("workflow", min: 0, max: (uint)workflowList.Count());
            yield return _workflowFactory.GenerateWorkflow(workflowList[(int) selection], null);
        }
    }
}
