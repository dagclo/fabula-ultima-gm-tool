using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class InitiativeDifficulty : Label, IInitiativeSeedReader
{
	private InitiativeSeed _npcInitiativeCheck;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void OnNpcsReady(ICollection<NpcInstance> npcs)
    {   
        var npcCheck = npcs.Max(n => n.Template.Initiative);
        _npcInitiativeCheck.NpcInitiative = npcCheck;
        this.Text = npcCheck.ToString();
    }

    public void OnInitiativeSeedReady(InitiativeSeed seed)
    {
        _npcInitiativeCheck = seed;
    }
}
