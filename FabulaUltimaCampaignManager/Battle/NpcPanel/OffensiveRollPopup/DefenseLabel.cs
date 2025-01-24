using Godot;
using System;

public partial class DefenseLabel : Label
{
    public void HandleActionSet(string name, string type, string defense)
    {
        this.Text = $"{defense}:";
    }
}
