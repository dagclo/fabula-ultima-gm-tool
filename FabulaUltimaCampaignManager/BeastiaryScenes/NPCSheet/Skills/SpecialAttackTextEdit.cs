using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpecialAttackTextEdit : TextEdit
{
    public void HandleSkillSet(SignalWrapper<SkillTemplate> signal)
    {
        this.Text = signal.Value.Text;
    }
}
