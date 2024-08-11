using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class OtherActionDescriptionEdit : LineEdit
{
    private ActionTemplate _action;

    public void HandleActionSet(SignalWrapper<ActionTemplate> signal)
    {
        _action = signal.Value;        
    }


    public void HandleTextChanged(string text)
    {
        _action.Effect = text;
    }
}
