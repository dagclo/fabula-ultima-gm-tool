using Godot;

namespace Battle;

public partial class StatusEffectDisplay : TextureRect
{
    [Export]
    public StatusEffect Target { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void StatusChanged(BattleStatus status)
    {
        this.Visible = status.IsStatusInEffect(Target);
    }
}

