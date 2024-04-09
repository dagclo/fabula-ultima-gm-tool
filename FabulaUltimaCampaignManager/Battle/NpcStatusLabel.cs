using FirstProject.Encounters;
using Godot;
using System;

public partial class NpcStatusLabel : PanelContainer, INpcStatusReader
{
    [Export]
    public string Attribute { get; set; }

    private Label Value;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Value = (Label)FindChild("Text"); // force exception if not found
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void HandleStatusSet(BattleStatus status)
    {
        status.StatusChanged += OnStatusChanged;
        OnStatusChanged(status);
    }

    private void OnStatusChanged(BattleStatus status)
    {
        string text;
        switch (Attribute)
        {
            case "HP":
                text = status.CurrentHP.ToString();
                break;
            case "MP":
                text = status.CurrentMP.ToString();
                break;
            default:
                text = "unset";
                break;
        }
        Value.Text = text;
    }
}
