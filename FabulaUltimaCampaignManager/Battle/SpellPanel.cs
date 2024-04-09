using FabulaUltimaNpc;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Linq;

public partial class SpellPanel : PanelContainer
{
    private SpellTemplate _spell;
    private NpcInstance _instance;
    private BattleStatus _battleStatus;
    private MessagePublisher<NpcActionMessage> _messagePublisher;

    [Signal]
    public delegate void NotEnoughMPEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<NpcActionMessage>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void UpdateSpell(SpellTemplate spell, FirstProject.Npc.NpcInstance npc)
    {
        _spell = spell;
        _instance = npc;
        if (_spell == null) return;
        foreach (var reader in this.FindChildren("*").Where(c => c is ISpellReader))
        {
            var spellReader = reader as ISpellReader;
            spellReader.Read(_spell);
        }
    }

    internal void UpdateStatus(BattleStatus status)
    {
        _battleStatus = status;

    }

    public void OnUseSpell()
    {
        _battleStatus.CurrentMP -= _spell.MagicPointCost;
        _messagePublisher.Publish((new NpcActionMessage
        {
            Id = _spell.Id,
            Action = _spell.Name,
            Actor = _instance.InstanceName,
            Verb = "uses",
        }).AsMessage());
        if (_battleStatus.CurrentMP < _spell.MagicPointCost) EmitSignal(SignalName.NotEnoughMP);
    }
}
