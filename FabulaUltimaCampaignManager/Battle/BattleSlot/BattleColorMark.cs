using Godot;

public partial class BattleColorMark : ColorRect
{
	public void HandleColorMarkSet(Color color)
	{
		this.Color = color;
	}
}
