using FabulaUltimaNpc;
using Godot;
using System.Linq;

public partial class ResistancePanel : PanelContainer, IBeastAttribute
{
    public delegate void ResistanceChangedEventHandler(string affinity);

    public event ResistanceChangedEventHandler ResistanceChanged;

    [Export]
    public string ResistanceName { get; set; }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (var child in this.FindChildren("*", "Control").Where(l => l is IResistanceReceiver))
        {
            var control = child as IResistanceReceiver;
            this.ResistanceChanged += control.HandleResistanceChanged;
            
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        if (ResistanceName == null) return;
        var resistance = beastTemplate.Resistances[ResistanceName];        
        ResistanceChanged?.Invoke(resistance.Affinity);
    }
}
