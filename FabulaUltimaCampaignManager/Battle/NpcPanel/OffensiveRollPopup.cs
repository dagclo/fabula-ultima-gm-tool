using FabulaUltimaGMTool.Battle;
using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Campaign;
using FirstProject.Encounters;
using FirstProject.Messaging;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class OffensiveRollPopup : PopupPanel, IAttackReader, INpcReader, ISpellReader
{
    private NpcInstance _npc;
    private string _actionType = string.Empty;
    private string _attribute1;
    private string _attribute2;
    private int _accMod;
    private int _damageMod;
    private Action _deregister;

    [Signal]
    public delegate void OnNpcTargetListUpdateEventHandler(Godot.Collections.Array<NpcInstance> npcList);

    [Signal]
    public delegate void OnActionUpdateEventHandler(string name, string type, string defense);

    [Signal]
    public delegate void OnCheckModelSetEventHandler(SignalWrapper<ICheckModel> signal);

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
        var otherNpcs = initialize.Value.Npcs.Select(p => p.npc).Where(n => n.Id != _npc.Id );
        var statuses = initialize.Value.Npcs.Select(p => p.status);
        var npcStatus = statuses.Single(s => s.ToString() == _npc.Id);
        CallDeferred(MethodName.Update, new Godot.Collections.Array<NpcInstance>(otherNpcs), npcStatus);
        return Task.CompletedTask;
    }

    private void Update(Godot.Collections.Array<NpcInstance> npcs, BattleStatus status)
    {        
        EmitSignal(SignalName.OnNpcTargetListUpdate, npcs);        
        var factory = new CheckFactory(_npc.Template, status, true);        
        var checkModel = factory.GetCheck(_actionType, _attribute1.ToLowerInvariant().FirstCharToUpper(), _attribute2.ToLowerInvariant().FirstCharToUpper());
        checkModel.AccuracyMod = _accMod;
        checkModel.HighRollMod = _damageMod;
        EmitSignal(SignalName.OnCheckModelSet, new SignalWrapper<ICheckModel>(checkModel));
    }

    public void ReadAttack(BasicAttackTemplate attack)
    {
        _actionType = "Attack";
        _attribute1 = attack.Attribute1;
        _attribute2 = attack.Attribute2;
        _accMod = attack.AccuracyMod;
        _damageMod = attack.DamageMod;
        this.Title = $"{_actionType} {attack.Name} Roll";
        EmitSignal(SignalName.OnActionUpdate, attack.Name, _actionType, "Def");
    }

    public void HandleNpcChanged(NpcInstance npc)
    {
        _npc = npc;
    }

    public void Read(SpellTemplate spellTemplate)
    {
        _actionType = "Spell";
        _attribute1 = spellTemplate.Attribute1;
        _attribute2 = spellTemplate.Attribute2;
        _accMod = 0; // need to figure out based on skills and level
        _damageMod = 0; // same as above.
        this.Title = $"{_actionType} Roll";
        EmitSignal(SignalName.OnActionUpdate, spellTemplate.Name, _actionType, "M Def");
        
    }

    public void Read(IBeastTemplate beast)
    {
        //throw new System.NotImplementedException();
    }

    public void HandleOpen()
    {
        this.Visible = true;
    }
}
