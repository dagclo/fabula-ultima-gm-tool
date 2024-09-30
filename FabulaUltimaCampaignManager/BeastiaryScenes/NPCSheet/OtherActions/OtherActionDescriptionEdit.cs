using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class OtherActionDescriptionEdit : TextEdit
{
    private ActionTemplate _action;

    public void HandleActionSet(SignalWrapper<ActionTemplate> signal)
    {
        _action = signal.Value;
        Text = _action.Effect;
    }


    public void HandleTextChanged()
    {
        _action.Effect = Text;
    }
}
