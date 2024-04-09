using FirstProject.Messaging;
using Godot;
using System;
using System.Linq;

public partial class BattlePlayerPanel : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var runState = GetNode<RunState>("/root/RunState");
		var players = runState.Campaign.Players.Where(p => p.IsValid).ToArray();
		var playerPanels = this.FindChildren("*").Where(n => n is IPlayerStatus).Take(players.Count()).Select(p => (IPlayerStatus) p);
		foreach((var player, var panel) in players.Zip(playerPanels))
		{
			panel.ReadPlayer(player);
		}       
    }    
}
