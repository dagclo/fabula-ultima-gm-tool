using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class OtherActionEntry : PanelContainer
{
    public ActionTemplate Action { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Action == null) return;
        foreach (var child in this.FindChildren("*", "Label").Where(c => c is IActionReceiver))
        {
            var skillReceiver = child as IActionReceiver;
            skillReceiver.HandleSkillChanged(Action);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    internal void UpdateAction(ActionTemplate action)
    {
        if (action == null) return;
        Action = action;
        foreach (var child in this.FindChildren("*", "Label").Where(c => c is IActionReceiver))
        {
            var skillReceiver = child as IActionReceiver;
            skillReceiver.HandleSkillChanged(Action);
        }
    }
}
