using FirstProject.Messaging;
using Godot;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public partial class CurrentTurnOwner : PanelContainer
{
    private Label _label;    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveNextTurnMessage);
        _label = (Label)this.FindChild("Text");
        _label.Text = string.Empty;
    }

    private Task ReceiveNextTurnMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState> turnDoneMessage))
        {
            throw new Exception("wrong message type");
        }        
           
        _label.CallDeferred(Label.MethodName.SetText, turnDoneMessage.Value.CurrentTurnOwner.Name);
        return Task.CompletedTask;
    }
}
