using FirstProject.Beastiary;
using Godot;
using System;

public partial class AccuracyMod : LineEdit
{
    private ICheckModel _checkModel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnActionSet(SignalWrapper<ICheckModel> signal)
    {
        _checkModel = signal.Value;
        Text = _checkModel.AccuracyMod?.ToString() ?? string.Empty;
    }

    public void OnSubmit(string newText)
    {
        if (_checkModel == null) return;
        if (!int.TryParse(newText, out var accuracyMod)) return;
        _checkModel.AccuracyMod = accuracyMod;
    }
}
