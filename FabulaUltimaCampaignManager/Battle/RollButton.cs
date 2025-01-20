using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;

public partial class RollButton : Button, INpcReader
{
    [Signal]
    public delegate void ResultReadyEventHandler(SignalWrapper<CheckResult> signalWrapper);

	private NpcInstance _instance;
    private ICheckModel _checkModel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
    }

	public void OnRoll()
	{
        if (!_checkModel.IsValid) return;
        var random = new RandomNumberGenerator();
        random.Randomize();
        var attribute1Result = random.RandiRange(1, _checkModel.Attribute1Die.Sides);
        var attribute2Result = random.RandiRange(1, _checkModel.Attribute2Die.Sides);
        var highRoll = attribute1Result > attribute2Result ? attribute1Result : attribute2Result;
        var accMod = _checkModel.AccuracyMod ?? 0;
        var result = attribute1Result + attribute2Result + accMod;
        var checkResult = new CheckResult
        {
            Success = result >= _checkModel.Difficulty,
            FinalHighRoll = highRoll + (_checkModel.HighRollMod ?? 0),
            HighRoll = highRoll,
            HighRollMod = _checkModel.HighRollMod ?? 0,
            ResultMod = accMod,
            TotalRoll = result,
            Attribute1Result = attribute1Result,
            Attribute2Result = attribute2Result,
            Attribute1Name = _checkModel.Attribute1Name,
            Attribute2Name = _checkModel.Attribute2Name,
        };
        EmitSignal(SignalName.ResultReady, new SignalWrapper<CheckResult>(checkResult));
	}

    public void OnActionSet(SignalWrapper<ICheckModel> signal)
    {
        if(_checkModel != null)
        {
            _checkModel.Changed -= this.OnCheckChanged; // make sure to prevent memory leaks
        }
        _checkModel = signal.Value;
        _checkModel.Changed += this.OnCheckChanged;     
    }

    private void OnCheckChanged()
    {
        if (!_checkModel.IsValid) return;
        this.Disabled = false;        
    }
}
