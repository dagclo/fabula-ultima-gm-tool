using FirstProject.Encounters;
using Godot;

public partial class NPCList : VBoxContainer
{

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
            this.AddChild(npcNode);
        }
    }
}
