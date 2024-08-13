using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;
using System.Linq;

public partial class SpecialAttackDetailLabel : RichTextLabel
{
    public void HandleAttackChanged(SignalWrapper<BasicAttackTemplate> signal)
    {
        var attack = signal.Value;
        var specialAttacks = attack.AttackSkills?.Where(s => s.OtherAttributes?.IsSpecialAttack ?? false) ?? Array.Empty<SkillTemplate>();
        this.Text = string.Join(System.Environment.NewLine, specialAttacks.Select(s => s.Text));
    }
}
