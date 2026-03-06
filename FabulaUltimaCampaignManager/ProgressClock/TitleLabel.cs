using Godot;

namespace FabulaUltimaGMTool.UI.ProgressClock;

public partial class TitleLabel : Label
{
	public void UpdateText(string text)
	{
		this.Text = text;
	}
}
