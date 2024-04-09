using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaDataImporter.Processor
{
    public interface IWorkflowFactory
    {
        IWorkflow GenerateWorkflow(WorkFlowKind kind, object? initValue);
        IEnumerable<WorkFlowKind> GetAllAvailableWorkFlows();
    }

    public class WorkflowFactory : IWorkflowFactory
    {
        private readonly IReadOnlyDictionary<WorkFlowKind, Func<object?, IWorkflowFactory, IWorkflow>> _workflowDictionary;

        public WorkflowFactory(IEnumerable<(WorkFlowKind kind, Func<object?, IWorkflowFactory, IWorkflow> func)> workflowGenerators)
        {
            _workflowDictionary = workflowGenerators.ToDictionary(p => p.kind, p => p.func);
        }

        public IEnumerable<WorkFlowKind> GetAllAvailableWorkFlows()
        {
            return _workflowDictionary.Keys;
        }

        public IWorkflow GenerateWorkflow(WorkFlowKind kind, object? initValue)
        {
            return _workflowDictionary[kind].Invoke(initValue, this);
        }
    }
}
