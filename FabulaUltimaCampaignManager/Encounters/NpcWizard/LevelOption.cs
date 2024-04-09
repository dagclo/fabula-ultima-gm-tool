using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Diagnostics.Metrics;
using System.Linq;

public partial class LevelOption : OptionButton, INpcReader
{
    private NpcInstance _instance;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		RemoveItem(0);
		foreach(var level in Enumerable.Range(5, 61)) //todo: set these as constants somewhere else
		{
			AddItem(level.ToString(), level);
		}
	}


    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
		var index = GetItemIndex(npc.Model.Level);
        this.Select(index);
    }

	public void OnItemSelected(int index)
	{
		var id = GetItemId(index);
		_instance.Model.Level = id;
	}
}
