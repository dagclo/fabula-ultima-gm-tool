using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class ResultLabel : Label, INpcReader, INpcStatusReader
{
    private ICheckModel _checkModel;    
    private BattleStatus _battleStatus;
    private MessagePublisher<EncounterLog> _messagePublisher;
    private NpcInstance _instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Text = string.Empty;
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public void OnResultReady(SignalWrapper<CheckResult> signalWrapper)
    {
        var checkResult = signalWrapper.Value;
        var successText = checkResult.Success ? "Success" : "Failed";
        var highRoll = checkResult.Success ? $" (hr: {checkResult.FinalHighRoll})" : string.Empty;
        _messagePublisher.Publish((new EncounterLog
        {
            Id = Guid.NewGuid(),
            Action = $"for {_checkModel.Action}",
            Actor = _instance.InstanceName,
            Verb = $"rolled {successText}",
        }).AsMessage());
        this.Text = $"{_instance.InstanceName} rolled a {successText} ({checkResult.TotalRoll}) {_checkModel.Action} check ({checkResult.Attribute1Result}, {checkResult.Attribute2Result}){highRoll}";
    }

    public void OnActionSet(SignalWrapper<ICheckModel> signal)
    {
        _checkModel = signal.Value;       
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
    }

    public void HandleStatusSet(BattleStatus status)
    {
        _battleStatus = status;
    }
}
