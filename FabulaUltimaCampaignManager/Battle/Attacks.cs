using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

namespace FirstProject.Battle
{
    public partial class Attacks : VBoxContainer, INpcReader
	{

        [Export]
        public PackedScene AttackScene { get; set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
                      
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }


        public void HandleNpcChanged(NpcInstance npc)
        {
            foreach (var child in this.FindChildren("AttackPanel", "PanelContainer"))
            {
                this.RemoveChild(child);
                child.QueueFree();
            }

            if (!npc.Template.AllAttacks.Any()) return;
            var beastTemplate = npc.Template;            
            foreach (var attack in beastTemplate.AllAttacks)
            {
                var scene = AttackScene.Instantiate<NpcAttack>();                
                this.AddChild(scene);
                scene.UpdateAttack(attack);
                scene.UpdateInstance(npc);
            }
        }
    }
}
