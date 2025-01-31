using Godot;

public partial class ActionNameLabel : Label
{
    public void HandleActionSet(string name, string type, string defense, string detail)
    {
        this.Text = $"{type} Name";
    }
}
