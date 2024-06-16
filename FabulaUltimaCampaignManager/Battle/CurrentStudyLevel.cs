using FirstProject.Encounters;
using Godot;
using System.Text;

public partial class CurrentStudyLevel : Label, INpcStatusReader
{
    [Export]
    public int NoInfo { get; set; } = 1;

    [Export]
    public int SomeInfo { get; set; } = 2;

    [Export]
    public int MostInfo { get; set; } = 3;

    [Export]
    public int AllInfo { get; set; } = 4;

    private BattleStatus _status { get; set; }

    public void HandleStatusSet(BattleStatus status)
    {
        _status = status;
    }

    public void OnSliderValueChanged(float value)
	{
		var labelText = new StringBuilder();
		var sliderInteger = (int)value;
        BattleStatus.StudyLevelEnum studyLevel = BattleStatus.StudyLevelEnum.NO_INFO;

        if (sliderInteger <= NoInfo)
		{
            labelText.Append("No Info");
        }

        if (sliderInteger >= SomeInfo)
        {
            labelText.Append("Rank, Species, Max HP, Max MP");
            studyLevel = BattleStatus.StudyLevelEnum.SOME_INFO;
        }

        if (sliderInteger >= MostInfo)
        {
            labelText.Append("Traits, Attributes, Defense, Magic Defense, Affinities");
            studyLevel = BattleStatus.StudyLevelEnum.MOST_INFO;
        }

        if (sliderInteger >= AllInfo)
        {
            labelText.Append("basic attacks and spells");
            studyLevel = BattleStatus.StudyLevelEnum.ALL_INFO;
        }
        this.Text = labelText.ToString();
        if (_status != null) _status.StudyLevel = studyLevel;
	}
}
