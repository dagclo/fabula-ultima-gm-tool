using FabulaUltimaGMTool.Battle.NpcPanel;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;

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
        var log = checkResult.ToEncounterLog(_checkModel.Action, _instance.InstanceName);
        _messagePublisher.Publish(log.AsMessage());
        this.Text = log.Action;
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
