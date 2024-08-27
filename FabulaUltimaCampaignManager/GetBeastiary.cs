using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System.Linq;
using System.Threading.Tasks;

public struct BeastiaryRefreshMessage
{

}

public partial class GetBeastiary : VBoxContainer
{   

    [Signal]
    public delegate void AddBeastToEncounterEventHandler(NpcInstance npc);

    [Signal]
    public delegate void LoadingBeastsEventHandler(bool loading);

    [Export]
    public PackedScene BeastEntryScene { get; set; }

    [Export]
    public PackedScene NpcWizard { get; set; }

    private CompositeSearchFilter<IBeastTemplate> _searchFilter = new CompositeSearchFilter<IBeastTemplate>();
    private MessagePublisher<BeastiaryRefreshMessage> _messagePublisher;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<BeastiaryRefreshMessage>(this.ReceiveRefreshMessage);
        _messagePublisher = messageRouter.GetPublisher<BeastiaryRefreshMessage>();
        CallDeferred(MethodName.Setup);
    }

    private Task ReceiveRefreshMessage(IMessage message)
    {
        if (!(message is IMessage<BeastiaryRefreshMessage> refreshMessage)) return Task.CompletedTask;
        CallDeferred(MethodName.Setup);
        return Task.CompletedTask;
    }

    private void Setup()
	{
        EmitSignal(SignalName.LoadingBeasts, true);
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
        foreach (var beast in dBAccess.Repository.GetBeasts().Where(b => _searchFilter.Apply(b)))
        {
            var node = BeastEntryScene.Instantiate<BeastEntryNode>();
            node.Beast = beast;
            this.AddChild(node);
            node.OnAddToEncounter += HandleAddEncounter;
            node.OnDeleteBeast += HandleDeleteBeast;
        }
        EmitSignal(SignalName.LoadingBeasts, false);
    }

    private void HandleAddEncounter(IBeastTemplate template)
    {
		var instance = new NpcInstance
		{	
			Model = new NpcModel(template.Model) // required to enable saving this using Godot
            {
                Level = template.Level
            }
		};

		var npcSheet = NpcWizard.Instantiate<NpcSheet>();        
        npcSheet.BeastModel = instance.Model;
        npcSheet.TitleOverride = "Add NPC to Encounter";
        npcSheet.NpcInstance = instance;        
        npcSheet.Closing += () => OnNpcSheetClose(npcSheet);
        npcSheet.OnSave += () => AddInstanceToEncounter(instance);
        this.AddChild(npcSheet);
    }

    private void HandleDeleteBeast(IBeastTemplate template)
    {
        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
        var repository = dBAccess.Repository;
        repository.DeleteBeastTemplate(template.Id);
        _messagePublisher.Publish(new BeastiaryRefreshMessage().AsMessage());
    }

    private void AddInstanceToEncounter(NpcInstance instance)
    {
        CallDeferred(MethodName.EmitSignal, SignalName.AddBeastToEncounter, instance);
    }

    //private void OnWizardCloseDefer(NpcSheet npcWizard)
    //{        
    //    CallDeferred(MethodName.OnWizardClose, npcWizard);
    //}

    private void OnNpcSheetClose(NpcSheet npcWizard)
    {
        RemoveChild(npcWizard);
        npcWizard.QueueFree();
    }

    public void HandledSearchFilterChanged(SignalWrapper<ISearchFilter<IBeastTemplate>> addSignal, SignalWrapper<ISearchFilter<IBeastTemplate>> removeSignal)
    {
        if(removeSignal.Value != null) _searchFilter.Filters.Remove(removeSignal.Value);
        if (addSignal.Value != null) _searchFilter.Filters.Add(addSignal.Value);
        _messagePublisher.Publish(new BeastiaryRefreshMessage().AsMessage());
    }
}
