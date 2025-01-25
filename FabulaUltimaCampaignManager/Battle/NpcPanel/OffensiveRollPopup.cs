using FabulaUltimaGMTool.Battle;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class OffensiveRollPopup : Window, IAttackReader, INpcReader, ISpellReader
{
    private NpcInstance _npc;
    private string _actionType = null;
    private string _attribute1;
    private string _attribute2;
    private int? _accMod;
    private int? _damageMod;
    private string _damageType;
    private string _detail;
    private string _offensiveActionName;
    private Action _deregister;
    private bool _enabled = false;

    [Signal]
    public delegate void OnNpcTargetListUpdateEventHandler(Godot.Collections.Array<NpcInstance> npcList);

    [Signal]
    public delegate void OnActionUpdateEventHandler(string name, string type, string defense, string detail);

    [Signal]
    public delegate void OnCheckModelSetEventHandler(SignalWrapper<ICheckModel> signal, BattleStatus status);

    [Signal]
    public delegate void OnResetEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{  
        var messageRouter = GetNode<MessageRouter>("/root/MessageRouter");
        _deregister = messageRouter.RegisterSubscriber<EncounterInitialize>(ReceiveMessage);
    }

    public void HandleTreeExiting()
    {
        _deregister?.Invoke();
    }

    public Task ReceiveMessage(IMessage message)
	{
        if (_npc == null || !(message is IMessage<EncounterInitialize> initialize)) return Task.CompletedTask;
        _enabled = true;
        var otherNpcs = initialize.Value.Npcs.Select(p => p.npc).Where(n => n.Id != _npc.Id );
        var statuses = initialize.Value.Npcs.Select(p => p.status);
        var npcStatus = statuses.Single(s => s.ToString() == _npc.Id);
        CallDeferred(MethodName.Update, new Godot.Collections.Array<NpcInstance>(otherNpcs), npcStatus);
        return Task.CompletedTask;
    }

    private void Update(Godot.Collections.Array<NpcInstance> npcs, BattleStatus status)
    {
        if (_actionType == null) return;
        EmitSignal(SignalName.OnNpcTargetListUpdate, npcs);        
        var factory = new CheckFactory(_npc.Template, status, true);        
        var checkModel = factory.GetCheck(_actionType, _attribute1?.ToLowerInvariant().ToCamelCase(), _attribute2?.ToLowerInvariant().ToCamelCase());
        checkModel.AccuracyMod = _accMod ?? _npc.Template.MagicCheckModifier;
        var currentMod = _damageMod ?? 0;
        checkModel.HighRollMod = currentMod + GetMagicalDamageBoost();
        checkModel.Difficulty = 0;
        checkModel.DamageType = _damageType ?? "";
        checkModel.Action = _offensiveActionName ?? checkModel.Action;
        EmitSignal(SignalName.OnCheckModelSet, new SignalWrapper<ICheckModel>(checkModel), status);
    }

    private int GetMagicalDamageBoost()
    {        
        var numApplied = _npc.Template.Skills.Count(s => s.Id == KnownSkills.ImprovedDamageSpell.Id);
        if (numApplied == 0) return 0;
        var damageBoost = _npc.Template.Skills.First(s => s.Id == KnownSkills.ImprovedDamageSpell.Id).OtherAttributes[DamageConstants.DAMAGE_BOOST];
        return int.Parse(damageBoost) * numApplied;
    }

    public void ReadAttack(BasicAttackTemplate attack)
    {
        _actionType = "Attack";
        _attribute1 = attack.Attribute1;
        _attribute2 = attack.Attribute2;
        _accMod = attack.AccuracyMod;
        _damageMod = attack.DamageMod;
        _damageType = attack.DamageType.Name;
        _offensiveActionName = attack.Name;
        if (attack.AttackSkills != null)
        {
            var stringBuilder = new StringBuilder();
            bool first = true;
            foreach (var skill in attack.AttackSkills.Where(s => s.OtherAttributes?.IsSpecialAttack == true))
            {
                if (!first)
                {
                    stringBuilder.Append("; ");
                }
                first = false;
                stringBuilder.Append(skill.Text);
            }
            _detail = stringBuilder.ToString();
        }
       
        this.Title = $"{_actionType} {attack.Name} Roll";
        EmitSignal(SignalName.OnActionUpdate, attack.Name, _actionType, "Def", _detail);
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npc = npc;
    }

    public void Read(SpellTemplate spellTemplate)
    {
        if (string.IsNullOrWhiteSpace(spellTemplate.Attribute1) || string.IsNullOrWhiteSpace(spellTemplate.Attribute2)) return;
        _actionType = "Spell";
        _attribute1 = spellTemplate.Attribute1;
        _attribute2 = spellTemplate.Attribute2;
        _accMod = null; // need to figure out based on skills and level
        _damageMod = spellTemplate.DamageModifier; // same as above.
        _damageType = spellTemplate.DamageType?.Name;
        _detail = spellTemplate.Description;
        _offensiveActionName = spellTemplate.Name;
        this.Title = $"{_actionType} Roll";
        EmitSignal(SignalName.OnActionUpdate, spellTemplate.Name, _actionType, "M Def", _detail);        
    }

    public void Read(IBeastTemplate beast)
    {
        //throw new System.NotImplementedException();
    }

    public void HandleOpen()
    {
        if (!_enabled) return;
        this.Visible = true;
    }

    public void HandleClosed()
    {
        this.Visible = false;
        EmitSignal(SignalName.OnReset);
    }
}
