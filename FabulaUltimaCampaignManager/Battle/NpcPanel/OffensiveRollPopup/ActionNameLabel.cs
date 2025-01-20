using Godot;
using System;

public partial class ActionNameLabel : Label
{
    public void HandleActionSet(string name, string type, string defense)
    {
        this.Text = $"{type} Name";
    }
}
