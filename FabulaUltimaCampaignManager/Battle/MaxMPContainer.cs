using Godot;

public partial class MaxMPContainer : HBoxContainer
{
    public void StudyLevelChanged(BattleStatus newStatus)
    {
        this.Visible = newStatus.StudyLevel >= BattleStatus.StudyLevelEnum.SOME_INFO;
    }
}
