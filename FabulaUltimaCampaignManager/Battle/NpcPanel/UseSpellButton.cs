using FabulaUltimaNpc;
using FirstProject.Battle;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;

public partial class UseSpellButton : Button
{	
	public void OnNotEnoughMP()
	{
        this.Disabled = true;
    }
    
}
