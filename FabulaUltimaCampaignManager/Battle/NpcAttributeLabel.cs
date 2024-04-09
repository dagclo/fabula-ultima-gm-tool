using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class NpcAttributeLabel : PanelContainer, INpcReader
{
	[Export]
	public string Attribute {  get; set; }

	private Label Value;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Value = (Label) FindChild("Text"); // force exception if not found
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {	
        Value.Text = npc.GetValueOf(Attribute);
    }
}
