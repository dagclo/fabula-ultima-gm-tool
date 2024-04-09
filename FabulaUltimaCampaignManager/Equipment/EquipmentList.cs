using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class EquipmentList : Label, IBeastAttribute
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Text = string.Join(", ", beastTemplate.Equipment.Select(e  => e.Name));
    }
}
