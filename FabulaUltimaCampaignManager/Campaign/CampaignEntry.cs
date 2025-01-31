using FirstProject.Beastiary;
using FirstProject.Campaign;
using Godot;
using System;

public partial class CampaignEntry : VBoxContainer
{
    public CampaignData Campaign { get; internal set; }
    public Action OnOpen { get; internal set; }

    [Signal]
    public delegate void OnCampaignUpdateEventHandler(CampaignData campaign);


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        CallDeferred(MethodName.UpdateCampaign);
	}

	public void UpdateCampaign()
    {
        EmitSignal(SignalName.OnCampaignUpdate, Campaign);
    }

    public void HandlePressed()
    {
        OnOpen?.Invoke();
    }
}
