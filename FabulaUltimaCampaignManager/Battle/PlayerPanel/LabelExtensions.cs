using Godot;

public static class LabelExtensions
{
    public static void SetFontColor(this Label label, bool flag, Color color)
    {
        if (flag && label.GetThemeColor("font_color") == color)
        {
            label.RemoveThemeColorOverride("font_color");
        }
        else if (!flag)
        {
            label.AddThemeColorOverride("font_color", color);
        }
    }
}
