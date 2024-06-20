using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;

public partial class ActionOptions : OptionButton, INpcReader, INpcStatusReader
{
	private CheckFactory _factory;
	private IBeastTemplate _beastTemplate;
	private BattleStatus _battleStatus;
    private readonly IDictionary<int,string> _indexToActionName = new Dictionary<int, string>();

    [Signal]
    public delegate void ActionSetEventHandler(SignalWrapper<ICheckModel> signal);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CreateCheckFactory()
	{
		if (_factory != null) return;
		if (_beastTemplate == null) return;
		if (_battleStatus == null) return;
		_factory = new CheckFactory(_beastTemplate, _battleStatus);
		int startIndex = this.ItemCount;
        foreach (var checkName in _factory.GetAvailableChecks())
		{
            this.AddItem(checkName, startIndex);
            _indexToActionName[startIndex] = checkName;
			startIndex++;
        }
        this.Disabled = false;
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _beastTemplate = npc.Template;
		CreateCheckFactory();
    }

    public void HandleStatusSet(BattleStatus status)
    {
		_battleStatus = status;
        CreateCheckFactory();
    }

    public void OnItemSelected(int index)
    {
        if (!_indexToActionName.ContainsKey(index)) return;
        var actionName = _indexToActionName[index];
        var check = _factory.GetCheck(actionName);
        EmitSignal(SignalName.ActionSet, new SignalWrapper<ICheckModel>(check));
    }
}
