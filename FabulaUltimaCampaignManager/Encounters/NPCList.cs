using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NPCList : VBoxContainer
{
    [Signal]
    public delegate void RemoveNpcEventHandler(NpcInstance encounter);

    [Export]
	public PackedScene EntryScene { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UpdateEncounter(Encounter encounter)
    {

        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }      
        
        foreach (var npc in encounter.NpcCollection)
        {
            var npcNode = EntryScene.Instantiate<NPCShortEntry>();
            npcNode.UpdateNpc(npc);
            npcNode.OnRemove += HandleRemove;
            this.AddChild(npcNode);
        }
    }

    public void HandleRemove(NpcInstance npc)
    {
        EmitSignal(SignalName.RemoveNpc, npc);
    }
}
