// See https://aka.ms/new-console-template for more information

using FabulaUltimaDatabase;
using FabulaUltimaDataImporter.IO;

internal class WorkflowRunner
{
    private readonly Instance _databaseInstance;
    private readonly UserIOWrapper _userIOWrapper;
    private readonly Queue<IWorkflow> _workflowQueue;
    private readonly IWorkflow _initialWorkflow;

    public WorkflowRunner(Instance db, UserIOWrapper userIOWrapper, IWorkflow initialWorkflow)
    {
        _databaseInstance = db;
        _userIOWrapper = userIOWrapper;
        _workflowQueue = new Queue<IWorkflow>();
        _initialWorkflow = initialWorkflow;
        Add(new[] { _initialWorkflow });
    }

    internal void Add(IEnumerable<IWorkflow> nextWorkFlows)
    {
        foreach(var workFlow in nextWorkFlows)
        {
            _workflowQueue.Enqueue(workFlow);
        }        
    }

    internal IEnumerable<IWorkflow> GetWorkFlowEnumerable()
    {
        while(_workflowQueue.Any())
        {
            var workflow = _workflowQueue.Dequeue();
            yield return workflow;
        }        
    }

    internal void Run()
    {
        foreach (IWorkflow workflow in GetWorkFlowEnumerable())
        {
            var nextWorkFlows = workflow.Run();
            Add(nextWorkFlows);
            if (!_workflowQueue.Any() && workflow.Kind != FabulaUltimaDataImporter.Processor.WorkFlowKind.END)
            {
                Add(new[] { _initialWorkflow });
            }
        }
    }
}