using FirstProject.Beastiary;
using FirstProject.Encounters;
using FirstProject.Npc;
using Godot;
using System.Collections.Generic;

public partial class HpProgressBar : TextureProgressBar, INpcReader
{

    [Export]
    public float SecondsToChange { get; set; } = 1;

    [Signal]
    public delegate void HpChangedEventHandler(SignalWrapper<ISet<HpState>> signalWrapper);

    public void HandleNpcChanged(NpcInstance npc)
    {
		this.MaxValue = npc.Template.HealthPoints;
        this.Value = MaxValue;        
    }

    private Tween _runningTween;
    public void StatusChanged(BattleStatus newStatus)
    {        
        if (Value == newStatus.CurrentHP) return;
        if (_runningTween != null) _runningTween.Kill();
        var stateList = new HashSet<HpState>();
        if (Value > newStatus.CurrentHP) 
        { 
            stateList.Add(HpState.HIT);
            if (newStatus.CurrentHP <= MinValue) stateList.Add(HpState.DYING);
        }
        else
        {
            stateList.Add(HpState.HEAL);
            if (Value <= MinValue) stateList.Add(HpState.REVIVING);
        }
        
        EmitSignal(SignalName.HpChanged, new SignalWrapper<ISet<HpState>>(stateList));
        _runningTween = CreateTween();
        _runningTween.TweenProperty(this, "value", newStatus.CurrentHP, SecondsToChange)
            .SetEase(Tween.EaseType.Out);
    }

    public void StudyLevelChanged(BattleStatus newStatus)
    {
        this.Visible = newStatus.StudyLevel >= BattleStatus.StudyLevelEnum.SOME_INFO;
    }
}
