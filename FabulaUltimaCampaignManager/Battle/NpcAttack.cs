using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class NpcAttack : PanelContainer
{
    private BasicAttackTemplate Attack { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void UpdateAttack(BasicAttackTemplate attack)
    {
        Attack = attack;
        if (Attack == null) return;
        foreach (var reader in this.FindChildren("*").Where(c => c is IAttackReader))
        {
            var attackReader = reader as IAttackReader;
            attackReader.ReadAttack(Attack);
        }
    }

    internal void UpdateInstance(NpcInstance npc)
    {
        foreach (var reader in this.FindChildren("*").Where(c => c is INpcReader))
        {
            var attackReader = reader as INpcReader;
            attackReader.HandleNpcChanged(npc);
        }
    }
}
