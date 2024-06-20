using Godot;
using System;
using System.Collections.Generic;

public partial class AffinityImage : TextureRect
{
    [Export]
    public Godot.Collections.Dictionary<string, Texture2D> Images { get; set; } = new Godot.Collections.Dictionary<string, Texture2D>();

    [Export]
    public Godot.Collections.Dictionary<string, Texture2D> GrayImages { get; set; } = new Godot.Collections.Dictionary<string, Texture2D>();


    public void HandleAffinityUpdate(string  affinityName)
    {
        Texture2D image;
        var key = affinityName.ToLowerInvariant();
        if (!Images.TryGetValue(key, out image)) return;
        this.Texture = image;
    }
}
