using FabulaUltimaGMTool.Adaptors;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpellClipboardButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, string speciesName)
    {
        var spellTemplate = signal.Value;
        var offensiveInfo = spellTemplate.IsOffensive ? $"[HR + {spellTemplate.DamageModifier}] {spellTemplate.DamageType.Name} damage" : string.Empty;
        var text = 
$"""
--------------
Spell
Name: {spellTemplate.Name}
MP Cost: {spellTemplate.MagicPointCost}
Target: {spellTemplate.Target}
Duration: {spellTemplate.Duration}
Is Offensive: {spellTemplate.IsOffensive}
tags: {speciesName}
Description: 
{spellTemplate.Description}
{offensiveInfo}
--------------
""";
        DisplayServer.ClipboardSet(text);
    }
}
