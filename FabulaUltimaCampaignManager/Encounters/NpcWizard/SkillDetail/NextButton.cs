using Godot;

namespace FabulaUltima.Encounters.NpcWizard.SkillDetail;

public partial class NextButton : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Disabled = true;
	}

    public void OnSkillsDefined(bool defined)
    {
        this.Disabled = !defined;
    }
}
