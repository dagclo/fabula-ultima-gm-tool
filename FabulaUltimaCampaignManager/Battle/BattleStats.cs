using Godot;
using System;

public partial class BattleStats : Control
{
    public void StudyLevelChanged(BattleStatus newStatus)
    {
        this.Visible = newStatus.StudyLevel >= BattleStatus.StudyLevelEnum.MOST_INFO;
    }
}
