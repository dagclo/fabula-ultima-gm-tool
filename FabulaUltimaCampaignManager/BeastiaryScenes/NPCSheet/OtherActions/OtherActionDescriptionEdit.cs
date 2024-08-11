using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class OtherActionDescriptionEdit : LineEdit
{
    public void HandleActionSet(SignalWrapper<ActionTemplate> signal)
    {
        var action = signal.Value;
        this.Text = action.Effect;
    }
}
