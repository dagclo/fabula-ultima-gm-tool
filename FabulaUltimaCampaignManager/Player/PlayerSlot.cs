using FirstProject.Beastiary;
using FirstProject.Campaign;
using Godot;
using System;
using System.Linq;

public partial class PlayerSlot : VBoxContainer
{
    private PlayerData _player;

    [Export]
	public int SlotIndex { get; set; }

    private Action<PlayerData> _onUpdatePlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        foreach (var node in this.FindChildren("*")
          .Where(l => l is IPlayerAttribute))
        {
            var child = node as IPlayerAttribute;
            _onUpdatePlayer += child.SetPlayer;
        }
        if (_player != null) _onUpdatePlayer?.Invoke(_player);
    }

    public void UpdateEncounter(SignalWrapper<CampaignData> signalWrapper)
    {
		var campaign = signalWrapper.Value;
		if (campaign == null) return;
		campaign.EnsurePlayerSlot(SlotIndex);
		_player = campaign.Players[SlotIndex];
        if (_player != null) _onUpdatePlayer?.Invoke(_player);
    }
}
