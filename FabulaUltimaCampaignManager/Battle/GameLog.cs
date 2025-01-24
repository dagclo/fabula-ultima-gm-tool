using FirstProject.Beastiary;
using FirstProject.Messaging;
using Godot;
using System.Threading.Tasks;

public partial class GameLog : RichTextLabel
{
    [Export]
    public int LagInMilliseconds { get; set; } = 30;

    [Export]
    public Color FailedColor { get; set; } = Colors.Red;

    [Export]
    public Color SuccessColor { get; set; } = Colors.Green;

    [Export]
    public Font PlayerWinFont { get; set; }

    [Export]
    public Font WhooshFont { get; set; }

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
        var action = typedMessage.Value;        
        CallDeferred(MethodName.HandleLog, new SignalWrapper<EncounterLog>(action));
        await ToSignal(GetTree().CreateTimer(LagInMilliseconds / 1000f), SceneTreeTimer.SignalName.Timeout);
    }

    public void HandleLog(SignalWrapper<EncounterLog> signal)
    {
        var action = signal.Value;        
        var color = GetLogColor(action.DisplayLevel);        
        PushColor(color);
        bool pushBold = false;
        if (action.DisplayLevel == DisplayLevel.CELEBRATE)
        {
            PushFont(PlayerWinFont);
        } 
        else if(action.DisplayLevel == DisplayLevel.WHOOSH)
        {
            PushFont(WhooshFont);
        }
        else
        {
            pushBold = true;
        }
        if(pushBold) PushBold();
        AppendText($"{action.Actor}");
        if (pushBold) Pop();
        AppendText($" {action.Verb} {action.Action}");        
        Newline();        
        PopAll();
    }

    private Color GetLogColor(DisplayLevel displayLevel)
    {
        switch(displayLevel)
        {
            case DisplayLevel.FAILED:
                return FailedColor;
            case DisplayLevel.SUCCESS: 
                return SuccessColor;
            default:
                return Colors.White;
        }
    }
}
