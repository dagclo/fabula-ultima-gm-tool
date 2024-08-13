using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class AttributeOptions : OptionButton, INpcReader, INpcStatusReader
{
	[Export]
	public bool IsAttribute1 { get; set; } = true;

    private ICheckModel _checkModel;
    private IBeastTemplate _template;
    private BattleStatus _battleStatus;
    private static readonly IDictionary<string, int> _attributeToIndex = new[]
	{
		string.Empty,
		nameof(IBeastTemplate.Dexterity),
		nameof(IBeastTemplate.Insight),
		nameof(IBeastTemplate.Might),
		nameof(IBeastTemplate.WillPower)
	}.Select((n, i) => (n, i))
		.ToDictionary(p => p.n, p => p.i);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		foreach(var pair in _attributeToIndex)
		{
            this.AddItem(pair.Key, pair.Value);
        }
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnActionSet(SignalWrapper<ICheckModel> signal)
	{	
		_checkModel = signal.Value;        
		var index = _attributeToIndex[IsAttribute1 ? _checkModel.Attribute1Name : _checkModel.Attribute2Name];

		Select(index);
	}

    public void OnItemSelected(int index)
    {
		if (_checkModel == null) return;
		if (_template == null) return;
        if (_battleStatus == null) return;
        var value = GetItemText(index);
		if(string.IsNullOrWhiteSpace(value)) return;
		if (IsAttribute1)
		{
			_checkModel.Attribute1Name = value;            
            _checkModel.Attribute1Die = _battleStatus.ApplyStatus(value, _template.GetDie(value));
        }
		else
		{
            _checkModel.Attribute2Name = value;
            _checkModel.Attribute2Die = _battleStatus.ApplyStatus(value, _template.GetDie(value));
        }
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
		_template = npc.Template;
    }

    public void HandleStatusSet(BattleStatus status)
    {
        _battleStatus = status;        
    }
}
