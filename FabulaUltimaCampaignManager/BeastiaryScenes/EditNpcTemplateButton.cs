using FabulaUltimaNpc;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EditNpcTemplateButton : Button, IBeastAttribute
{
    private IBeast _beastModel;
    private MessagePublisher<BeastiaryRefreshMessage> _messagePublisher;

    [Export]
    public PackedScene BeastEditScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<BeastiaryRefreshMessage>();
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Visible = beastTemplate.CanBeModified;
        _beastModel = beastTemplate.Model;
    }

    public void HandlePressed()
    {
        this.Disabled = true;
        var npcSheet = BeastEditScene.Instantiate<NpcSheet>();
        npcSheet.BeastModel = _beastModel;
        if(_beastModel is NpcModel npcModel)
        {
            npcSheet.TitleOverride = $"Edit {npcModel.Instance.InstanceName}";
            npcSheet.NpcInstance = npcModel.Instance;
        }
        else
        {
            npcSheet.TitleOverride = $"Edit {_beastModel.Name}";
        }
        
        npcSheet.Closing += () => HandleNpcSheetClose(npcSheet);
        this.AddChild(npcSheet);
    }

    private void HandleNpcSheetClose(NpcSheet npcSheet)
    {
        RemoveChild(npcSheet);
        npcSheet.QueueFree();
        this.Disabled = false;
        //_messagePublisher.Publish(new BeastiaryRefreshMessage().AsMessage());
        BeastTemplateAction?.Invoke(new[] { BeastEntryNode.Action.CHANGED }.ToHashSet());
    }
}
