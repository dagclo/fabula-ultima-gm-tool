using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class UseAttackButton : Button, IAttackReader, INpcReader
{	
    private string _instanceName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

    }

    public void ReadAttack(BasicAttackTemplate attack)
    {
        
    }

	public void OnUseAttack()
	{

    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instanceName = npc.InstanceName;
    }
}
