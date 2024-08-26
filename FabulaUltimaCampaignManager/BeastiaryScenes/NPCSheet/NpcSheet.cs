using FabulaUltimaDatabase.Models;
using FabulaUltimaGMTool;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NpcSheet : Window
{
    private Resolver _skillResolver;
    private BeastiaryRepository _beastRepository;

    [Signal]
    public delegate void SkillSlotsAvailableEventHandler(int skillSlotsAvailable);

    public Action Closing { get; internal set; }
    public Action<ISet<BeastEntryNode.Action>> OnBeastChanged { get; private set; }
    public IBeast BeastModel { get; internal set; }
    public NpcInstance NpcInstance { get; set; }
    public Action OnSave { get; internal set; }
    public string TitleOverride { get; internal set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.Title = string.IsNullOrWhiteSpace(TitleOverride) ? this.Title : TitleOverride;
        _skillResolver = GetNode<SkillResolver>("/root/SkillResolver").Instance;
        _beastRepository = GetNode<DbAccess>("/root/DbAccess").Repository;
        var beast = BeastModel ?? new BeastModel()
        {
            Id = Guid.NewGuid(),
            Level = 5
        };
        BeastModel = beast;
        foreach (var child in this.FindChildren("*", recursive: true)
           .Where(l => l is BeastEntryNode))
        {
            var node = child as BeastEntryNode;
            node.Beast = new SkilledBeastTemplateWrapper(new BeastTemplate(beast));
            node.OnTrigger += HandleTrigger;
            if(OnSave != null) node.OverrideSave += OnSave;
            this.OnBeastChanged = node.ActionTemplate;
        }
    }

    private void HandleTrigger(IBeastTemplate template)
    {        
        var editableBeastTemplate = template as SkilledBeastTemplateWrapper;
        var editableBeastModel = editableBeastTemplate?.Model as BeastModel;
        if (editableBeastModel == null) return;
        if (editableBeastModel.Species != null) // no point in resolving without species
        {
            editableBeastTemplate.UpdateSkills();
            var input = new SkillInputData
            {
                MaxMP = editableBeastTemplate.Internal.MagicPoints,
                MaxHP = editableBeastTemplate.Internal.HealthPoints,
                MDefMod = 0,
                DefMod = 0,
                DefOverride = null,
            };
            // remove old resolved skills
            var oldResolvedSkills = editableBeastModel.Skills.Where(s => s.IsResolved()).ToArray();
            foreach (var oldSkill in oldResolvedSkills)
            {
                editableBeastModel.Skills.Remove(oldSkill);
            }

            var resolverResults = _skillResolver.ResolveSkills(editableBeastTemplate, input);
            var resolvedSkills = resolverResults.SkillSlots.Where(s => s?.skill != null && s.Value.skill.IsAffinitySkill()).Select(s => s.Value.skill).ToArray();

            EmitSignal(SignalName.SkillSlotsAvailable, resolverResults.RemainingSkillPoints - editableBeastModel.Skills.Count(s => !s.IsAffinitySkill()));

            foreach(var newResolvedSkills in resolvedSkills)
            {
                editableBeastModel.Skills.Add(newResolvedSkills);
            }

            foreach (var affinityGroup in editableBeastTemplate.Skills.Where(s => s.IsAffinitySkill()).GroupBy(s => s.Id).Where(g => g.Count() > 1).ToArray())
            {
                //for now save resolved skills
                var versionToTake = affinityGroup.FirstOrDefault(s => s.IsResolved()) ?? affinityGroup.First();
                foreach (var skill in affinityGroup)
                {
                    editableBeastTemplate.Model.Skills.Remove(skill);
                }
                editableBeastTemplate.Model.Skills.Add(versionToTake);
            }
        }        
      
        this.OnBeastChanged.Invoke(new HashSet<BeastEntryNode.Action> { BeastEntryNode.Action.CHANGED });
    }

	public void HandleCloseRequested()
	{
        _beastRepository.ClearQueuedUpdate(BeastModel.Id);
        Closing?.Invoke();
	}

    public void HandleInstanceNameChange(string newText)
    {
        if (NpcInstance == null) return;
        NpcInstance.InstanceName = newText;
        this.OnBeastChanged.Invoke(new HashSet<BeastEntryNode.Action> { BeastEntryNode.Action.CHANGED });
    }
}
