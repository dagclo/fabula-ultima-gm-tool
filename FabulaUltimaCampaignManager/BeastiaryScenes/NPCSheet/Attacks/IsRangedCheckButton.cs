using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class IsRangedCheckButton : CheckButton
{
    private BasicAttackTemplate _basicAttack;

    public void HandleToggle(bool toggledOn)
	{
        if(_basicAttack == null) return;
        _basicAttack.IsRanged = toggledOn;
    }

    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        _basicAttack = attack;
    }
}
