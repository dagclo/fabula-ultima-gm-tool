using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InitiativePopup : PopupPanel
{
    private PackedScene _targetScene;
    private Encounter _targetEncounter;

    [Signal]
    public delegate void SwitchSceneEventHandler();

    public Action<ICollection<NpcInstance>> NpcsReady { get; private set; }
    public Action<InitiativeSeed> InitiativeSeedReady { get; private set; }
    public Action<PackedScene> TargetSceneReady { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Hide();
        
        foreach(var child in this.FindChildren("*").Where(c => c is IInitiativeSeedReader))
        {
            var node = child as IInitiativeSeedReader;
            this.InitiativeSeedReady += node.OnInitiativeSeedReady;
            node.OnSubmit += HandleSubmit;
            if(child is InitiativeDifficulty initDiff)
            {
                this.NpcsReady += initDiff.OnNpcsReady;
            }

            if (child is RunEncounterButton runButton)
            {
                this.TargetSceneReady += runButton.OnTargetSceneReady;
            }
        }
    }

    internal void OnStartEncounter(PackedScene scene, Encounter encounter)
    {
        _targetScene = scene;
        _targetEncounter = encounter;
        var runState = GetNode<RunState>("/root/RunState");
        var playerCount = runState.Campaign.Players.Where(p => p.IsValid).Count();
        _targetEncounter.InitiativeSeed = new InitiativeSeed
        {
            NumPlayers = playerCount,
        };
        InitiativeSeedReady?.Invoke(encounter.InitiativeSeed);
        NpcsReady?.Invoke(encounter.NpcCollection);
        this.TargetSceneReady(_targetScene);
        this.Show();
    }

    public void HandleSubmit()
    {        
        this.Hide();
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        var _ = messageRouter.TearDown();
        EmitSignal(SignalName.SwitchScene);
    }
}
