using FirstProject.Messaging;
using Godot;
using System;

public partial class ResetRoundButton : Button
{
    private MessagePublisher<RoundState> _roundStateMessagePublisher;
    private MessagePublisher<EncounterLog> _logMessagePublisher;
    private int _roundNumber;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");        
        _roundStateMessagePublisher = messageRouter.GetPublisher<RoundState>();
        _logMessagePublisher = messageRouter.GetPublisher<EncounterLog>();
        _roundNumber = 1;
    }

    public void HandlePressed()
    {
        _roundStateMessagePublisher.Publish(new RoundState { RoundNumber = ++_roundNumber /* we want the next value*/ }.AsMessage());
        _logMessagePublisher.Publish(new EncounterLog { DisplayLevel = DisplayLevel.WHOOSH, Actor = "Round", Verb = _roundNumber.ToString(), Action = "Start" }.AsMessage());
    }

}
