using FirstProject.Encounters;
using Godot;
using System;

public partial class GroupCheck : LineEdit, IInitiativeSeedReader
{
    private InitiativeSeed _initiativeSeed;
    private string _curText;

    public Action OnSubmit { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnInitiativeSeedReady(InitiativeSeed seed)
    {
        _initiativeSeed = seed;
    }

    public void OnTextChanged(string newText)
    {
        if (_initiativeSeed == null)
        {
            GD.PushError("GroupCheck received input before its initiative seed was set");
            return;
        }
        if (string.IsNullOrWhiteSpace(newText))
        {
            _curText = string.Empty;
            _initiativeSeed.PlayerCheck = -1; // no input -> seed invalid -> Run disabled
            return;
        }
        if(!int.TryParse(newText, out var checkNum) || checkNum < 0)
        {
            // reject the keystroke: restore the last accepted text
            this.Text = _curText;
            this.CaretColumn = _curText?.Length ?? 0;
            return;
        }
        _initiativeSeed.PlayerCheck =  checkNum;
        _curText = newText;
    }

    public void HandleTextSubmitted(string _)
    {
        // Enter must respect the same validity gate as the Run button
        if (_initiativeSeed?.IsValid != true) return;
        OnSubmit?.Invoke();
    }
}
