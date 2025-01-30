using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class VillainsList : VFlowContainer
{
    private CampaignData _campaign;
    private MessagePublisher<SaveMessage> _messagePublisher;

    [Export]
    public PackedScene VillianEntryScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<SaveMessage>();
    }

    public void HandleCampaignChanged(SignalWrapper<CampaignData> signal)
    {
        _campaign = signal.Value;
        _campaign.Villains = _campaign.Villains ?? new Godot.Collections.Array<NpcInstance>();

        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        foreach (var villian in _campaign.Villains)
        {            
            villian.VillainStats.Changed += HandleVillainChanged;
            villian.Changed += HandleVillainChanged;
            var npcNode = VillianEntryScene.Instantiate<VillianEntry>();
            npcNode.UpdateNpc(villian);
            npcNode.OnRemove += (NpcInstance instance) => HandleRemove(instance, npcNode);
            this.AddChild(npcNode);
        }
    }

    private void HandleVillainChanged()
    {
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

    private void HandleRemove(NpcInstance instance, VillianEntry entry)
    {
        instance.VillainStats.Changed -= HandleVillainChanged;
        _campaign.Villains.Remove(instance);
        RemoveChild(entry);
        entry.QueueFree();
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (!(data.As<GodotObject>() is NpcInstance npc)) return false;
        return true;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var original = data.As<NpcInstance>();        
        var villianNode = VillianEntryScene.Instantiate<VillianEntry>();        
        villianNode.OnRemove += (NpcInstance instance) => HandleRemove(instance, villianNode);
        villianNode.UpdateNpc(original);
        this.AddChild(villianNode);
        _campaign.Villains.Add(original);
        _messagePublisher.Publish((new SaveMessage()).AsMessage());
    }
}
