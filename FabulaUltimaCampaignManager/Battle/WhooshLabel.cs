using FirstProject.Messaging;
using Godot;
using System.Threading.Tasks;

public partial class WhooshLabel : Label
{
    [Export]
    public double EntranceTimeInSeconds { get; set; } = 3;
    [Export]
    public double ExitTimeInSeconds { get; set; } = 1;

    [Export]
    public double StopTimeInSeconds { get; set; } = 0;

    [Export]
    public double CallbackWaitSeconds { get; set; } = 5;

    [Export]
    public float TravelDistance { get; set; } = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<EncounterLog>(this.ReceiveMessage);        
        _messagePublisher = messageRouter.GetPublisher<EncounterEnd>();
        this.Modulate = Colors.Transparent;
    }

    private Tween _runningTween;
    private MessagePublisher<EncounterEnd> _messagePublisher;

    private Task ReceiveMessage(IMessage message)
    {
        if (!(message is IMessage<EncounterLog> typedMessage)) return Task.CompletedTask;
        var log = typedMessage.Value;
        if(log.DisplayLevel == DisplayLevel.DEFAULT) return Task.CompletedTask;
        var runText = log.ToString();        
        CallDeferred(MethodName.RunTween, runText, log.DisplayLevel == DisplayLevel.CELEBRATE);
        return Task.CompletedTask;
    }   

    private void RunTween(string runText, bool endEncounter)
    {
        if (_runningTween != null) _runningTween.Kill();
        this.Text = runText;
        var curXOffSet = this.Size.X / 2;
        var offScreenStart = new Vector2(-this.Size.X, this.Position.Y);
        this.Position = offScreenStart; // reset position
        var halfTravelDistance = TravelDistance / 2;
        var midPoint = new Vector2(offScreenStart.X + curXOffSet + halfTravelDistance, this.Position.Y);
        var endPoint = new Vector2(midPoint.X + halfTravelDistance + curXOffSet, this.Position.Y);

        _runningTween = CreateTween().SetTrans(Tween.TransitionType.Quad);       
       
        _runningTween.Parallel().TweenProperty(this, "position", midPoint, EntranceTimeInSeconds)
            .SetEase(Tween.EaseType.Out);
        _runningTween.Parallel().TweenProperty(this, "modulate", Colors.White, EntranceTimeInSeconds)
            .SetEase(Tween.EaseType.Out);
        _runningTween.SetParallel(false);
        _runningTween.TweenProperty(this, "position", midPoint, StopTimeInSeconds)
           .SetEase(Tween.EaseType.Out);
        _runningTween.TweenProperty(this, "position", endPoint, ExitTimeInSeconds)
          .SetEase(Tween.EaseType.In);
        _runningTween.SetParallel(true);
        _runningTween.TweenProperty(this, "modulate", Colors.Transparent, ExitTimeInSeconds)            
            .SetEase(Tween.EaseType.In);
        
        if (!endEncounter) return;
        _runningTween.SetParallel(false);
        _runningTween.TweenProperty(this, "position", endPoint, CallbackWaitSeconds)
          .SetEase(Tween.EaseType.In);
        var callable = new Callable(this, MethodName.SendEndMessage);
        _runningTween.TweenCallback(callable);
    }

    private void SendEndMessage()
    {
        _messagePublisher.Publish(new EncounterEnd().AsMessage());
    }
}
