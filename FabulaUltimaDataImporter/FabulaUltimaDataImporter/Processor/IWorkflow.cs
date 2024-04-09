// See https://aka.ms/new-console-template for more information

using FabulaUltimaDataImporter.Processor;

public interface IWorkflow
{
    string GetName { get; }
    WorkFlowKind Kind { get; }    
    IEnumerable<IWorkflow> Run();
}