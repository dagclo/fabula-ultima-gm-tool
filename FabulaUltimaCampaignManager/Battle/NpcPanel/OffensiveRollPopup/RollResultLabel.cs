using FabulaUltimaDatabase;
using FabulaUltimaGMTool.Battle.NpcPanel;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class RollResultLabel : RichTextLabel, INpcReader
{
    private MessagePublisher<EncounterLog> _messagePublisher;
    private NpcInstance _instance;
    private ICheckModel _checkModel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Text = string.Empty;
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

    public void HandleReset()
    {
        this.Text = string.Empty;
    }

    public void OnResultReady(SignalWrapper<CheckResult> signalWrapper)
    {
        var checkResult = signalWrapper.Value;
        var log = checkResult.ToEncounterLog(_checkModel.Action, _instance.InstanceName);
        _messagePublisher.Publish(log.AsMessage());
        this.Text = string.Empty;
        this.AppendText(checkResult.Success ? "[color=green]Success[/color]" : "[color=red]Failed[/color]");
        this.TooltipText = log.Action;
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
    }

    public void OnActionSet(SignalWrapper<ICheckModel> signal)
    {  
        _checkModel = signal.Value;     
    }
}
