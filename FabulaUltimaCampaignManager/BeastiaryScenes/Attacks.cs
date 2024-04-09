using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class Attacks : VBoxContainer, IBeastAttribute
{
    [Export]
    public PackedScene AttackNodeScene { get; set; }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        foreach(var child in this.FindChildren("*", owned: false).Where(c => c is BasicAttack)) //instantiated scenes aren't "owned"
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        
        foreach(var attack in beastTemplate.AllAttacks)
        {            
            var scene = AttackNodeScene.Instantiate<BasicAttack>();
            scene.BasicAttackObject = attack;
            this.AddChild(scene);
        }
    }
}
