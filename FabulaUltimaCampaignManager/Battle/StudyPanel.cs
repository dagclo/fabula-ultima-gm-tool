using FirstProject.Beastiary;
using Godot;
using System;

public partial class StudyPanel : Control
{
	[Export]
	public BattleStatus.StudyLevelEnum VisibleStudyLevel { get; set; } = BattleStatus.StudyLevelEnum.ALL_INFO;
    public void HandleStudyLevelChanged(SignalWrapper<BattleStatus.StudyLevelEnum> studyLevel)
	{
		this.Visible = studyLevel.Value >= VisibleStudyLevel;
    }
}
