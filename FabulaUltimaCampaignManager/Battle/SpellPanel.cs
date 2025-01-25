using FabulaUltimaNpc;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System.Linq;

public partial class SpellPanel : PanelContainer
{
    private SpellTemplate _spell;
    private NpcInstance _instance;
    private BattleStatus _battleStatus;
    private MessagePublisher<EncounterLog> _messagePublisher;

    [Signal]
    public delegate void NotEnoughMPEventHandler();

    [Signal]
    public delegate void OnCastOffensiveSpellEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _messagePublisher = messageRouter.GetPublisher<EncounterLog>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    internal void UpdateSpell(SpellTemplate spell, NpcInstance npc)
    {
        _spell = spell;
        _instance = npc;
        if (_spell == null) return;
        foreach (var reader in this.FindChildren("*").Where(c => (c is ISpellReader) || (c is INpcReader)))
        {
            if(reader is ISpellReader spellReader)
            {
                spellReader.Read(_spell);
                spellReader.Read(_instance.Template);
            }            
            
            if(reader is INpcReader npcReader)
            {
                npcReader.HandleNpcChanged(_instance);
            }
        }
    }

    internal void UpdateStatus(BattleStatus status)
    {
        _battleStatus = status;

    }

    public void OnUseSpell()
    {
        if (_spell.IsOffensive && _spell.DamageType != null && _spell.DamageModifier != null)
        {
            EmitSignal(SignalName.OnCastOffensiveSpell);
        }
        else
        {
            _battleStatus.CurrentMP -= _spell.MagicPointCost;
            _messagePublisher.Publish((new EncounterLog
            {
                Id = _spell.Id,
                Action = _spell.Name,
                Actor = _instance.InstanceName,
                Verb = "casts",
            }).AsMessage());
        }
       
        if (_battleStatus.CurrentMP < _spell.MagicPointCost) EmitSignal(SignalName.NotEnoughMP);
    }
}
