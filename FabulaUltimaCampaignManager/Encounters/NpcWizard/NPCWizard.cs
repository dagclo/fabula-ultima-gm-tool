using FabulaUltimaDatabase;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class NPCWizard : Window
{
    private NpcInstance _instance;
    private Action<NpcInstance> _updateNpc;
	public Action Closing { get; set; }
    public Action<NpcInstance> InstanceSet { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		foreach(var node in this.FindChildren("*").Where(c => c is INpcReader))
		{
			var npcReader = node as INpcReader;
			_updateNpc += npcReader.HandleNpcChanged;
        }		
    }

    public void OnAddBeastToEncounter(NpcInstance instance)
	{
		_instance = instance;
		_updateNpc?.Invoke(instance);
		Show();
    }

	public void OnClose()
	{
		Closing?.Invoke();
    }
	public void OnComplete()
	{
		InstanceSet?.Invoke(_instance);
		CallDeferred(MethodName.OnClose);
    }
}
