using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class BeastScroller : ScrollContainer, INpcReader
{
    private NpcInstance _instance;
    private BeastEntryNode _beastEntry;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_beastEntry = GetChild<BeastEntryNode>(0); // assuming BeastEntry only child
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
		_instance.Changed += OnInstanceChanged;
		_beastEntry.Beast = _instance.Template;
    }

    private void OnInstanceChanged()
    {
        _beastEntry.Beast = _instance.Template;
    }
}
