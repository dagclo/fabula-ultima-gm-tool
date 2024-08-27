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

    public void OnResultReady(SignalWrapper<CheckResult> signalWrapper)
    {
        var checkResult = signalWrapper.Value;
        var successText = checkResult.Success ? "Successfully with" : "Failed with ";
        var highRoll = checkResult.Success ? $"[hr+mod]=[{checkResult.HighRoll}+{checkResult.HighRollMod}={checkResult.FinalHighRoll}" : string.Empty;
        var detailString = $"[{checkResult.Attribute1Name}+{checkResult.Attribute2Name}+mod]=[{checkResult.Attribute1Result}+{checkResult.Attribute2Result}+{checkResult.ResultMod}]=[{checkResult.TotalRoll}]";
        _messagePublisher.Publish((new EncounterLog
        {
            Id = Guid.NewGuid(),
            Action = $"rolled",
            Actor = _instance.InstanceName,
            Verb = $"{_checkModel.Action} with {successText}|{detailString}|{highRoll}",
        }).AsMessage());
        this.Text = $"{_instance.InstanceName} rolled for {_checkModel.Action} {successText} | {detailString} | {highRoll}";
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
