using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Npc;
using Godot;

public partial class GetBeastiary : VBoxContainer
{

    [Signal]
    public delegate void AddBeastToEncounterEventHandler(NpcInstance npc);

    [Export]
    public PackedScene BeastEntryScene { get; set; }

    [Export]
    public PackedScene NpcWizard { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        CallDeferred(MethodName.Setup);
	}

	private void Setup()
	{
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
        foreach (var beast in dBAccess.Repository.GetBeasts())
        {
            var node = BeastEntryScene.Instantiate<BeastEntryNode>();
            node.Beast = beast;
            this.AddChild(node);
            node.OnAddToEncounter += HandleAddEncounter;
            node.OnDeleteBeast += HandleDeleteBeast;
        }
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

		var npcWizard = NpcWizard.Instantiate<NPCWizard>();
        this.AddChild(npcWizard);
        npcWizard.OnAddBeastToEncounter(instance);
        npcWizard.Closing += () => OnWizardClose(npcWizard);
        npcWizard.InstanceSet += AddInstanceToEncounter;
    }

    private void HandleDeleteBeast(IBeastTemplate template)
    {
        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
        var repository = dBAccess.Repository;
        repository.DeleteBeastTemplate(template.Id);
        CallDeferred(MethodName.Setup);
    }

    private void AddInstanceToEncounter(NpcInstance instance)
    {
        CallDeferred(MethodName.EmitSignal, SignalName.AddBeastToEncounter, instance);
    }

    private void OnWizardCloseDefer(NPCWizard npcWizard)
    {        
        CallDeferred(MethodName.OnWizardClose, npcWizard);
    }

    private void OnWizardClose(NPCWizard npcWizard)
    {
        RemoveChild(npcWizard);
        npcWizard.QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
