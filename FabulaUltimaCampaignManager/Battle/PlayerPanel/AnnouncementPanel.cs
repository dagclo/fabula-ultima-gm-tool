using Godot;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public partial class AnnouncementPanel : PanelContainer
{
    private float _maxAlpha;

	[Export]
	public float MinAlpha { get; set; } = 0;

    [Export]
    public double TimeToAppear { get; set; } = 1;

    [Export]
    public double TimeToShow { get; set; } = 3;

    [Export]
    public double TimeToHide { get; set; } = 1;

    public double TotalTime => TimeToAppear + TimeToShow + TimeToHide;

    [Signal]
    public delegate void WaitTimeSetEventHandler(double waitTime);

    private Color AdjustAlpha(float alpha) => new Color(this.Modulate.R, this.Modulate.G, this.Modulate.B, alpha);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _maxAlpha = this.Modulate.A;
        if (MinAlpha >= _maxAlpha) throw new Exception("min alpha needs to be less than max alpha");		
		var transparentColor = AdjustAlpha(this.MinAlpha);
		this.Modulate = transparentColor;
        EmitSignal(SignalName.WaitTimeSet, TotalTime);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(_state)
		{
			case State.HIDDEN:
				//this.Modulate = AdjustAlpha(MinAlpha);
				break;
            case State.START_SHOWING:
				InterpolateAlphaUp(delta);
                break;
            case State.SHOWING:
                if (!_showingTimerRunning) CallDeferred(MethodName.StartShowingTimer);
                break;
			case State.START_HIDING:
				InterpolateAlphaDown(delta);
				break;
            default:
				throw new Exception("unhandled state");
		}
	}

    private ConcurrentQueue<double> _waitTimes = new ConcurrentQueue<double>();
    private bool _showingTimerRunning = false;
    private async void StartShowingTimer()
	{       
        _showingTimerRunning = true;
        while (_waitTimes.TryDequeue(out var waitTime))
        {   
            await Task.Delay(TimeSpan.FromSeconds(waitTime));            
        }
        _showingTimerRunning = false;
        _state = State.START_HIDING;
        _secondsLeftToHide = TimeToHide;
    }

	private void InterpolateAlphaUp(double delta)
	{
        _secondsLeftToShow = Math.Clamp(_secondsLeftToShow - delta, 0, TimeToAppear);
        var percentageLeft = (_secondsLeftToShow / TimeToAppear);
        if (percentageLeft == 0)
        {
            this._state = State.SHOWING;            
            return;
        }
        var alphaDiff = _maxAlpha - this.MinAlpha;
        var interpolatedAlpha = _maxAlpha - (percentageLeft * alphaDiff);
        this.Modulate = AdjustAlpha((float)interpolatedAlpha);        
    }

    private void InterpolateAlphaDown(double delta)
    {
        _secondsLeftToHide = Math.Clamp(_secondsLeftToHide - delta, 0, TimeToHide);
        var percentageLeft = (_secondsLeftToHide / TimeToHide);
        if (percentageLeft == 0)
        {
            this._state = State.HIDDEN;
            return;
        }
        var alphaDiff = _maxAlpha - this.MinAlpha;
        var interpolatedAlpha =  (percentageLeft * alphaDiff) + MinAlpha;
        this.Modulate = AdjustAlpha((float)interpolatedAlpha);
    }

    private double _secondsLeftToShow;
    private double _secondsLeftToHide;

    public void OnMessageReceived(double showTime)
	{   
		_secondsLeftToShow = TimeToAppear;
		if(_state != State.SHOWING) _state = State.START_SHOWING;
        _waitTimes.Enqueue(TimeToShow);
    }

	private State _state = State.HIDDEN;
	private enum State
	{
		START_SHOWING,
		SHOWING,
		START_HIDING,
		HIDDEN
	}
}
