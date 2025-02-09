using Godot;
using System;

public static class WindowExtensions
{
    public static void ResizeForResolution(this Window window)
    {
         // figure out window size
        
        var screenSize = DisplayServer.ScreenGetSize();        
        var yMultiplier = screenSize.Y / 1080;        
        window.ContentScaleFactor = Math.Clamp( yMultiplier, 0.5f, 8);
        window.Size = new Vector2I( window.Size.X * yMultiplier, window.Size.Y * yMultiplier);
        
    }
}
