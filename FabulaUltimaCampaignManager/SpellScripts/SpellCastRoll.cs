using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class SpellCastRoll : Label, ISpellReader
{
    private int _accuracyBonus;

	public void Read(SpellTemplate spellTemplate)
	{
		string roll;
		if (!string.IsNullOrWhiteSpace(spellTemplate.Attribute1) && !string.IsNullOrWhiteSpace(spellTemplate.Attribute2))
		{
            var checkModifier = _accuracyBonus == 0 ? string.Empty : $" + {_accuracyBonus}";
            roll = $"[{spellTemplate.Attribute1.ShortenAttribute()} + {spellTemplate.Attribute2.ShortenAttribute()}]{checkModifier}";
        }
		else
		{
			roll = string.Empty;
		}
		this.Text = roll;
	}

	public void Read(IBeastTemplate beast)
	{
		// do nothing
		_accuracyBonus = beast.MagicCheckModifier;
	}
}
