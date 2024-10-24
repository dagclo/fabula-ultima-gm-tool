using FabulaUltimaGMTool;
using FabulaUltimaGMTool.Adaptors;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpellQrCode : TextureRect
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, string speciesName)
	{
		var spellTemplate = signal.Value;
		var data = PHSAdapter.ToDataFormat(spellTemplate, speciesName);
		var texture = data.ToQRCode();
		this.Texture = texture;
    }
}
