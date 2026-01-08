using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Sprite : Sprite2D, INpcReader
{
    private AnimationPlayer _animationPlayer;    
    private State _state = State.ACTIVE;
    private State _defaultState = State.ACTIVE;    
    private NpcInstance _instance;

    [Export]
    public int MaxWidth { get; set; } = 250;

    [Export]
    public int MaxHeight { get; set; } = 250;

    [Export]
    public string IdleAnimation { get; set; } = "default";

    [Export]
    public string ActiveAnimation { get; set; } = "active";

    [Export]
    public string HitAnimation { get; set; } = "hit";

    [Export]
    public string DyingAnimation { get; set; } = "dying";

    [Export]
    public string RevivingAnimation { get; set; } = "reviving";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        messageRouter.RegisterSubscriber<RoundState>(this.ReceiveRoundStateMessage);        
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

    private async Task ReceiveRoundStateMessage(IMessage message)
    {
        if (!(message is IMessage<RoundState>))
        {
            throw new Exception("wrong message type");
        }

        int timeLeftInMs = 500000000; // way too long
        const int timeStep = 50;
        while(_instance == null && timeLeftInMs > 0)
        {            
            await Task.Delay(timeStep); // dislike this a lot
            timeLeftInMs -= timeStep;
        }

        _state = State.ACTIVE;
        _defaultState = _state;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        switch (_state)
        {
            case State.IDLE:
                _animationPlayer.Play(IdleAnimation);
                break;
            case State.ACTIVE:
                _animationPlayer.Play(ActiveAnimation);
                break;
            case State.HIT:
                if (_animationPlayer.CurrentAnimation != HitAnimation) _state = _defaultState;
                break;
            case State.DEATH:
                break;
            case State.REVIVAL:
                if (_animationPlayer.CurrentAnimation != RevivingAnimation) _state = _defaultState;
                break;
            default:
                // do nothing;
                break;
        }
        
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _instance = npc;
        if (string.IsNullOrWhiteSpace(npc?.Template.ImageFile)) return;
        var imageFile = npc.Template.ImageFile;
        var texture = (Texture2D) GD.Load(imageFile);
        this.Texture = texture;

        // rescale to fit
        float scaleFactor;
        if(texture.GetWidth() > texture.GetHeight())
        {
            scaleFactor = CalculateScaleFactor(MaxWidth, texture.GetWidth());
        }
        else
        {
            scaleFactor = CalculateScaleFactor(MaxHeight, texture.GetHeight());
        }
        this.ApplyScale(new Vector2(scaleFactor, scaleFactor));         
    }

    private static float CalculateScaleFactor(int max, float value)
    {
        if (max >= value) return 1;
        return max / value;
    }

    
    private MessagePublisher<EncounterLog> _messagePublisher;

    public void HandleStatusChanged(BattleStatus status)
    {   
        _state = status.IsActive ? State.ACTIVE : State.IDLE;
        _defaultState = _state;    
    }

    public void OnHpChanged(SignalWrapper<ISet<HpState>> signal)
    {
        var hpState = signal.Value;
        if(hpState.Contains(HpState.HIT)) 
        {   
            _state = State.HIT;
            _animationPlayer.Play(HitAnimation);            
        }

        //if (hpState.Contains(HpState.HEAL))
        //{
        //    _state = State.HIT;
        //    _animationPlayer.Play(HitAnimation);
        //}

        if (hpState.Contains(HpState.DYING))
        {
            _state = State.DEATH;
            _animationPlayer.Queue(DyingAnimation);
            _messagePublisher.Publish(new EncounterLog 
            { 
                Actor = _instance.InstanceName,
                Verb  = "has died",
            }.AsMessage());
        }
        else if (_state == State.ACTIVE && hpState.Contains(HpState.REVIVING)) // this should be gone
        {
            _messagePublisher.Publish(new EncounterLog
            {
                Actor = _instance.InstanceName,
                Verb = "is back!",
            }.AsMessage());
            _state = State.REVIVAL;
            _animationPlayer.Queue(RevivingAnimation);
        }
        else
        {
            _animationPlayer.Queue(IdleAnimation);
        }
    }

    [Flags]
    private enum State
    {
        ACTIVE = 0,
        HIT = 1,
        IDLE = 2,
        DEATH = 4,
        REVIVAL = 8
    }
}
