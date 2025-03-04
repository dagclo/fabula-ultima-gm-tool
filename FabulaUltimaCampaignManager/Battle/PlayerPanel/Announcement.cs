using FirstProject.Messaging;
using Godot;
using System.Linq;
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
        messageRouter.RegisterSubscriber<EncounterLog>(this.ReceiveMessage);
        this.Text = string.Empty;        
    }

    private async Task ReceiveMessage(IMessage message)
    {   
        if (!(message is IMessage<EncounterLog> typedMessage)) return;        
        var log = typedMessage.Value;
        var objectString = string.IsNullOrWhiteSpace(log.Object) ? string.Empty : $"on {log.Object}";
        var logActionNoExtraneousDetails = log.Action?.Split('|').FirstOrDefault() ?? string.Empty;
        var announcement = $"{log.Actor} {log.Verb} {logActionNoExtraneousDetails} {objectString}";
        CallDeferred(MethodName.SetText, announcement);        
        CallDeferred(MethodName.EmitSignal, SignalName.MessageReceived, _waitBetweenMessage);
        await ToSignal(GetTree().CreateTimer(_waitBetweenMessage), SceneTreeTimer.SignalName.Timeout); // adjust timing later
    }

    public void OnWaitTimeSet(double wait)
    {
        _waitBetweenMessage = wait;
    }
}
