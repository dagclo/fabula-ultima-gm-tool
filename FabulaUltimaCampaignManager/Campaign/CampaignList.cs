using FirstProject;
using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;

public partial class CampaignList : VBoxContainer
{
    private MessagePublisher<CampaignUpdate> _messagePublisher;

    [Export]
	public PackedScene CampaignScene { get; set; }

    [Export]
    public Configuration Configuration { get; set; }

    [Signal]
    public delegate void OnCampaignChosenEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<CampaignUpdate>();
    }

    public void HandleVisibilityChanged(bool targetVisible)
    {
        if (!targetVisible) return;
        // remove any existing children
        foreach (var child in this.GetChildren())
        {
            this.RemoveChild(child);
            child.QueueFree();
        }

        foreach(var campaign in Configuration.GetCampaigns())
        {
            var campaignScene = CampaignScene.Instantiate<CampaignEntry>();
            campaignScene.Campaign = campaign;
            campaignScene.OnOpen += () => HandleCampaignOpen(campaign);
            this.AddChild(campaignScene);
        }
    }

    private void HandleCampaignOpen(CampaignData campaignData)
    {
        EmitSignal(SignalName.OnCampaignChosen);
        _messagePublisher.Publish(new CampaignUpdate { CampaignData = campaignData }.AsMessage());
    }
}
