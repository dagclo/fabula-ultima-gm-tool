using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class ActionNameEdit : LineEdit
{
    private ActionTemplate _action;

    public void HandleActionSet(SignalWrapper<ActionTemplate> signal)
	{
		_action = signal.Value;
        Text = _action.Name;
    }

	public void HandleTextChanged(string text)
	{
        _action.Name = text;
    }
}
