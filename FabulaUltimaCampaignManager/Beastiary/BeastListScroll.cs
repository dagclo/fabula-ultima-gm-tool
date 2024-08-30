using FirstProject.Messaging;
using Godot;
using System.Threading.Tasks;

public partial class BeastListScroll : ScrollContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<BeastiaryRefreshMessage>(this.ReceiveRefreshMessage);
    }

    private Task ReceiveRefreshMessage(IMessage message)
    {
        if (!(message is IMessage<BeastiaryRefreshMessage> refreshMessage)) return Task.CompletedTask;        
        CallDeferred(MethodName.SetVScroll, (int)GetVScrollBar().MinValue);
        return Task.CompletedTask;
    }   
}
