using FabulaUltimaGMTool.BeastiaryScenes;
using Godot;
using System.Collections.Generic;

public partial class SkillSlotsCountLabel : Label, IValidatable
{
    private int? _slotsLeft;

    string IValidatable.Name => "Skill Slots";

    public void HandleSkillSlotsLeft(int slots)
	{
        _slotsLeft = slots;
		this.Text = $"Skill Slots Left: {slots}";
	}

    public IEnumerable<TemplateValidation> Validate()
    {
        if (_slotsLeft == null) yield break;
        if (_slotsLeft > 0) yield return new TemplateValidation { Level = ValidationLevel.WARNING, Message = $" {_slotsLeft} skills left" };
    }
}
