using FirstProject.Campaign;
using FirstProject.Messaging;
using Godot;
using System;

public partial class NewCampaignLineEdit : LineEdit
{
    private MessagePublisher<CampaignUpdate> _messagePublisher;

    [Signal]
    public delegate void OnTextValidEventHandler(bool isValid);

    [Signal]
    public delegate void OnNewCampaignCreatedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<CampaignUpdate>();
    }

    public void HandleTextChanged(string newText)
	{
        if (string.IsNullOrWhiteSpace(Text))
        {
            EmitSignal(SignalName.OnTextValid, false);
            return;
        }
        EmitSignal(SignalName.OnTextValid, true);
	}

    public void HandlePressed()
    {
        var campaignData = new CampaignData
        {
            Name = Text,
            Id = Guid.NewGuid().ToString()
        };
        EmitSignal(SignalName.OnNewCampaignCreated);
        _messagePublisher.Publish(new CampaignUpdate { CampaignData = campaignData }.AsMessage());        
        Clear();
    }
}
