using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class DamageButton : Button, INpcStatusReader, INpcReader
{
    private int? _damage;
    private FirstProject.Npc.Affinity? _affinity;
    private string _damageType;
    private BattleStatus _battleStatus;
    private MessagePublisher<EncounterLog> _messagePublisher;
    private IBeastTemplate _template;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Disabled = true;
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnDamageChanged(int damage)
    {
        _damage = damage;
        SetDisabled();
    }

    private void SetDisabled()
    {
        if (_damage == null || _affinity == null || _template == null)
        {
            this.Disabled = true;
            return;
        }
        Disabled = false;
    }

    public void OnButtonPressed()
    {
        switch(_affinity)
        {
            case Affinity.HEAL:
            case Affinity.ABSORBS:
                _battleStatus.CurrentHP = Math.Min(_template.HealthPoints, _battleStatus.CurrentHP + (_damage ?? 0));
                break;
            case Affinity.MP_DAMAGE:
                _battleStatus.CurrentMP = Math.Max(0, _battleStatus.CurrentMP - (_damage ?? 0));
                break;
            default:
                _battleStatus.CurrentHP = Math.Max(0, _battleStatus.CurrentHP - (_damage ?? 0));
                break;
        }

        _messagePublisher.Publish((new EncounterLog
        {
            Id = Guid.NewGuid(), // todo: decide what to do with this
            Action = $"of {_damageType} {Math.Abs(_damage ?? 0)} (affinity: {_affinity})",
            Actor = "Players", // todo: set current player
            Verb = $"{Text}",
        }).AsMessage());
    }

    public void HandleStatusSet(BattleStatus status)
    {
		_battleStatus = status;
    }

    public void OnDamageTypeChanged(SignalWrapper<FirstProject.Npc.Affinity> signal, string damageType)
    {
        _affinity = signal.Value;
        _damageType = damageType;
        string verb;
        switch(_affinity)
        {
            case Affinity.HEAL:
            case Affinity.ABSORBS:
                verb = "Healing";
                break;
            case Affinity.MP_DAMAGE:
                verb = "MP Loss";
                break;
            default:
                verb = "Damage";
                break;
        }
        
        Text = $"Apply {verb}";
        SetDisabled();
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _template = npc.Template;
    }
}