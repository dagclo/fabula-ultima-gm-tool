using FirstProject.Messaging;
using Godot;
using System;

public partial class PlayersWinButton : Button
{
    private MessagePublisher<EncounterLog> _messagePublisher;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

	public void HandlePressed()
	{        
        var log = new EncounterLog
        {
            Action = "",
            Id = Guid.NewGuid(),
            Verb = "Win!",
            Actor = "Players",
            DisplayLevel = DisplayLevel.CELEBRATE
        };
        _messagePublisher.Publish(log.AsMessage());
    }
}
