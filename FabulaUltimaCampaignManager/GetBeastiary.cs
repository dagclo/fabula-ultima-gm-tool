using FabulaUltimaNpc;
using FirstProject;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

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
		// remove example beast
		var childNode = this.GetChildren().Single();
		this.RemoveChild(childNode);
        childNode.QueueFree();

        CallDeferred(MethodName.Setup);
	}

	private void Setup()
	{
        var dBAccess = GetNode<DbAccess>("/root/DbAccess");
        foreach (var beast in dBAccess.Repository.GetBeasts())
        {
            var node = BeastEntryScene.Instantiate<BeastEntryNode>();
            node.Beast = beast;
            this.AddChild(node);
            node.OnAddToEncounter += HandleAddEncounter;
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
