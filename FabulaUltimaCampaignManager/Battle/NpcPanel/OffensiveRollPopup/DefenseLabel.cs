using Godot;

public partial class DefenseLabel : Label
{
    public void HandleActionSet(string name, string type, string defense, string detail)
    {
        this.Text = $"{defense}:";
    }
}
