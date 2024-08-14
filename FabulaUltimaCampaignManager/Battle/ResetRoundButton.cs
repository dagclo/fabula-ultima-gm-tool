using FirstProject.Messaging;
using Godot;
using System;

public partial class ResetRoundButton : Button
{
    private MessagePublisher<RoundState> _messagePublisher;
    private int _roundNumber;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");        
        _messagePublisher = messageRouter.GetPublisher<RoundState>();
        _roundNumber = 1;
    }

    public void HandlePressed()
    {
        _messagePublisher.Publish(new RoundState { RoundNumber = ++_roundNumber /* we want the next value*/ }.AsMessage());
    }

}
