// See https://aka.ms/new-console-template for more information

using CommandLine;
using CommandLine.Text;
using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Configuration;
using FabulaUltimaDatabase.Models;
using FabulaUltimaDataImporter.IO;
using FabulaUltimaDataImporter.Processor;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;

var result = Parser.Default.ParseArguments<Options>(args)
    .WithNotParsed(errors =>
    {
        var sentenceBuilder = SentenceBuilder.Create();
        foreach (var error in errors)
            Console.WriteLine(sentenceBuilder.FormatError(error));
    });

if (result.Value == null)
{
    Console.WriteLine("parsing failed");
    return;
}

var options = result.Value;

Console.WriteLine("starting import");

var databaseConfiguration = new DatabaseConfiguration()
{
    FileName = options.Filename,
    Mode = options.Create ? Microsoft.Data.Sqlite.SqliteOpenMode.ReadWriteCreate : Microsoft.Data.Sqlite.SqliteOpenMode.ReadWrite
};

var db = new Instance(databaseConfiguration);
if(options.ReloadKnownSkills)
{
    Console.WriteLine("uploading known skills");
    db.UpdateKnownSkills(KnownSkills.GetAllKnownSkills());
}

var specialAttackIndex = new SpecialAttackIndex(db);
var skillResolver = new Resolver(db, specialAttackIndex);

UserIOWrapper userIOWrapper;

if(string.IsNullOrWhiteSpace(options.InputTape))
{
    userIOWrapper = new UserIOWrapper();
}
else
{
    var fileLines = File.ReadLines(options.InputTape); 
    userIOWrapper = new InputTapeIO(fileLines);
}

var workflows = new List<(WorkFlowKind kind, Func<object?, IWorkflowFactory, IWorkflow> func)>
{ 
   (WorkFlowKind.BEAST, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new BeastWorkflow(db, userIOWrapper, f))),
   (WorkFlowKind.INITIAL, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new StartingWorkflow(userIOWrapper, f))),
   (WorkFlowKind.ATTACK, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new AttackWorkflow(db, userIOWrapper){ InitialAttack = (BasicAttackEntry) o })),
   (WorkFlowKind.SPELL, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new SpellWorkflow(db, userIOWrapper, f){ InitialSpell = (SpellEntry) o })),
   (WorkFlowKind.EQUIPMENT, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new EquipmentWorkflow(db, userIOWrapper){ InitialEquipment = (EquipmentEntry) o })),
   (WorkFlowKind.SKILL, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new SkillWorkflow(db, userIOWrapper, skillResolver){ SkillInputData = (SkillInputData) o })),
   (WorkFlowKind.ACTION, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new ActionWorkflow(db, userIOWrapper){ InitialAction = (ActionEntry) o })),
   (WorkFlowKind.END, new Func<object?, IWorkflowFactory, IWorkflow>((o, f) => new EndWorkflow(userIOWrapper))),
};

var workflowFactory = new WorkflowFactory(workflows);   

var workflowRunner = new WorkflowRunner(db, userIOWrapper, workflowFactory.GenerateWorkflow(WorkFlowKind.INITIAL, null));
workflowRunner.Run();


