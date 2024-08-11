using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class AddedActionEntry : VBoxContainer
{
    [Signal]
    public delegate void ActionSetEventHandler(SignalWrapper<ActionTemplate> action);

    [Signal]
    public delegate void BeastSetEventHandler(SignalWrapper<IBeastTemplate> beast);

    public ActionTemplate Action { get; internal set; }
    public Action OnUpdateBeast { get; internal set; }
    public Action<AddedActionEntry> OnRemoveAction { get; internal set; }

    internal void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (Action == null) return;
        EmitSignal(SignalName.BeastSet, new SignalWrapper<IBeastTemplate>(beastTemplate));
    }

    public void HandleRemoveSpell()
    {
        OnRemoveAction?.Invoke(this);
    }

    public void HandleUpdateBeast()
    {
        OnUpdateBeast?.Invoke();
    }
}
