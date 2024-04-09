using FirstProject.Messaging;
using Godot;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public partial class GameLog : Label
{
    [Export]
    public int LagInMilliseconds { get; set; } = 30;    

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
        var log = $"{action.Actor} {action.Verb} {action.Action}";
        CallDeferred(MethodName.AppendLine, log);
        await ToSignal(GetTree().CreateTimer(LagInMilliseconds / 1000f), SceneTreeTimer.SignalName.Timeout);
    }

    public void AppendLine(string line)
    {
        this.Text += $"{line}\n"; // using \n because the windows \r\n\ doesn't work
    }
}
