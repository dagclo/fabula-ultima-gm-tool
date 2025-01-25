using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System;

public partial class RollButton : Button, INpcReader, ISpellReader
{
    [Signal]
    public delegate void ResultReadyEventHandler(SignalWrapper<CheckResult> signalWrapper);

	private NpcInstance _instance;
    private ICheckModel _checkModel;
    private BattleStatus _status;
    private int? _mpCost;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Disabled = true;
	}

    public void HandleTreeExiting()
    {
        if(_status != null) _status.Changed -= HandleNpcStatusChanged;
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		_instance = npc;
    }

    public void HandleReset()
    {
        this.Disabled = true;
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
            Target = _checkModel.Target,
        };
        EmitSignal(SignalName.ResultReady, new SignalWrapper<CheckResult>(checkResult));
	}

    public void OnActionSet(SignalWrapper<ICheckModel> signal, BattleStatus status)
    {
        if(_checkModel != null)
        {
            _checkModel.Changed -= this.OnCheckChanged; // make sure to prevent memory leaks
        }
        _checkModel = signal.Value;
        _checkModel.Changed += this.OnCheckChanged;
        _status = status;
        _status.Changed += HandleNpcStatusChanged; 
    }

    private void HandleNpcStatusChanged()
    {
        if (_mpCost != null) return;
        _status.CurrentMP -= _mpCost.Value;
    }

    private void OnCheckChanged()
    {
        if (!_checkModel.IsValid) return;
        this.Disabled = false;        
    }

    public void Read(SpellTemplate spellTemplate)
    {
        _mpCost = spellTemplate.MagicPointCost;
    }

    public void Read(IBeastTemplate beast)
    {
        // do nothing
    }
}
