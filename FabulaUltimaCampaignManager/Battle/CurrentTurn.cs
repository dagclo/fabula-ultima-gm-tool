using FirstProject.Messaging;
using Godot;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public partial class CurrentTurn : PanelContainer
{
    private Label _label;
    private ConcurrentQueue<string> _valueToUpdate = new ConcurrentQueue<string>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveNextTurnMessage);
        _label = (Label) this.FindChild("Text");
        _label.Text = string.Empty;
    }

    private Task ReceiveNextTurnMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> turnDoneMessage))
        {
            throw new Exception("wrong message type");
        }
        
        _valueToUpdate.Enqueue(turnDoneMessage.Value.TurnNumber.ToString()); // have to do this because this isn't run on the main thread
        return Task.CompletedTask;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_valueToUpdate.TryDequeue(out var val))
        {
            _label.Text = val;
        }
    }
}
