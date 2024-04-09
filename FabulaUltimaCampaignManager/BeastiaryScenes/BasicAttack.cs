using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class BasicAttack : HBoxContainer
{
    public BasicAttackTemplate BasicAttackObject { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (BasicAttackObject == null) return;
		foreach(var reader in  this.GetChildren().Where(c => c is IAttackReader))
		{
			var attackReader = reader as IAttackReader;
			attackReader.ReadAttack(BasicAttackObject);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
