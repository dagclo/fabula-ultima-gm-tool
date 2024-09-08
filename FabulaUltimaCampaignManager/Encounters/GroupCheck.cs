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
        if (_initiativeSeed == null) throw new Exception("not set");
        if (string.IsNullOrWhiteSpace(newText))
        {
            _curText = string.Empty;
            return;
        }
        if(!int.TryParse(newText, out var checkNum))
        {
            this.Text = _curText;
            return;
        }
        _initiativeSeed.PlayerCheck =  checkNum;
        _curText = newText;
    }

    public void HandleTextSubmitted(string _)
    {
        if (string.IsNullOrWhiteSpace(_curText)) return;
        OnSubmit?.Invoke();
    }
}
