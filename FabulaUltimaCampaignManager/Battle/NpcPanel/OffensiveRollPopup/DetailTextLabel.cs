using Godot;
using System;

public partial class DetailTextLabel : Label
{
    public void HandleActionSet(string name, string type, string defense, string detail)
    {
        this.Text = detail;
    }
}
