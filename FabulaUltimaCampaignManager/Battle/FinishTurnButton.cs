using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class FinishTurnButton : Button, INpcReader, INpcStatusReader
{
    private NpcInstance _instance;
    private BattleStatus _status;
    private string _configuredText;

    [Export]
    public string TurnCounterCharacter { get; set; } = "*";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _configuredText = this.Text;        
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		this._instance = npc;
    }

	public void OnRoundChanged()
	{
		if (this._instance == null) return;
        if (this._status == null) return;
        this.Disabled = false;
        _status.ResetTurns();
    }

	public void OnButtonPressed() 
	{
        _status.DecrementTurns();
        this.Disabled = !_status.IsActive;
	}

    public void HandleStatusSet(BattleStatus status)
    {
        _status = status;
        SetTitle();
        _status.StatusChanged += OnStatusChanged;
    }

    private void OnStatusChanged(BattleStatus status)
    {
        SetTitle();
    }

    private void SetTitle()
    {
        Text = $"{_configuredText} {string.Join("", Enumerable.Range(1, _status.NumTurnsLeft).Select(_ => TurnCounterCharacter))}";
    }
}
