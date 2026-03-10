using FabulaUltimaGMTool.Model.ProgressClock;
using FabulaUltimaGMTool.UI.ProgressClock;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ProgressClockList : Container
{
    private Array<ProgressClockModel> _progressClocks;
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
    public PackedScene Entry { get; set; }

    [Export]
    public PackedScene Dialog { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
    }

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        var campaign = signal.Value;
        if (campaign.ProgressClocks == null) campaign.ProgressClocks = new Godot.Collections.Array<ProgressClockModel>();
        _progressClocks = campaign.ProgressClocks;
        UpdateList();
    }

    private void UpdateList()
    {
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        if (_progressClocks?.Any() != true) return;
        foreach (var clock in _progressClocks)
        {
            //todo delete
            if(clock.SectionStates?.Any() != true)
            {
                clock.SectionStates = [false, false, false, false];
            }
            var scene = Entry.Instantiate<ProgressClockEntry>();
            scene.ProgressClock = clock;
            scene.OnShow += (ProgressClockModel m) => OnShow(m);            
            AddChild(scene);
            scene.Owner = this;
        }
    }

    private void OnShow(ProgressClockModel m)
    {
        OpenDialog(m);
    }

    private void HandleRemove(ProgressClockModel m)
    {
        throw new NotImplementedException();
    }

    private void HandleAdd()
    {
        var model = new ProgressClockModel
        {
            SectionStates = [false, false, false, false]
        };
        _progressClocks.Add(model);
        OpenDialog(model);
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
        CallDeferred(MethodName.UpdateList);
    }

    private void OpenDialog(ProgressClockModel model)
    {
        var dialog = Dialog.Instantiate<ProgressClock>();
        dialog.Model = model;
        dialog.OnHideClock += (ProgressClock c) => HandleHideClock(c);
        dialog.OnRemove += (ProgressClockModel m) => HandleRemove(m);
        dialog.OnSave += () => HandleSave();
        AddChild(dialog);
        dialog.Owner = this;
    }

    private void HandleSave()
    {
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

    private void HandleHideClock(ProgressClock c)
    {        
        CallDeferred(MethodName.UpdateList);
        RemoveChild(c);
        c.QueueFree();
    }
}
