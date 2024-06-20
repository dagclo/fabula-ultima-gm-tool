using FirstProject.Messaging;
using Godot;
using System.Threading.Tasks;

public partial class Announcement : Label
{
    private double _waitBetweenMessage = 1;

    [Signal]
    public delegate void MessageReceivedEventHandler(double showTime);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<NpcActionMessage>(this.ReceiveMessage);
        this.Text = string.Empty;        
    }

    private async Task ReceiveMessage(IMessage message)
    {   
        if (!(message is IMessage<NpcActionMessage> typedMessage)) return;        
        var action = typedMessage.Value;
        var log = $"{action.Actor} {action.Verb} {action.Action}\n"; // using \n because the windows \r\n\ doesn't work
        CallDeferred(MethodName.SetText, log);        
        CallDeferred(MethodName.EmitSignal, SignalName.MessageReceived, _waitBetweenMessage);
        await ToSignal(GetTree().CreateTimer(_waitBetweenMessage), SceneTreeTimer.SignalName.Timeout); // adjust timing later
    }

    public void OnWaitTimeSet(double wait)
    {
        _waitBetweenMessage = wait;
    }
}
