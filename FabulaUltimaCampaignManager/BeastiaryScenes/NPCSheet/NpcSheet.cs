using FabulaUltimaDatabase.Models;
using FabulaUltimaGMTool;
using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FabulaUltimaSkillLibrary.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class NpcSheet : Node
{
    private Resolver _skillResolver;

    public Action Closing { get; internal set; }
    public Action<ISet<BeastEntryNode.Action>> OnBeastChanged { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _skillResolver = GetNode<SkillResolver>("/root/SkillResolver").Instance;
        foreach (var child in this.FindChildren("*", recursive: true)
           .Where(l => l is BeastEntryNode))
        {
            var node = child as BeastEntryNode;
            var beast = new BeastModel();
            beast.Id = Guid.NewGuid();
            node.Beast = new SkilledBeastTemplateWrapper(new BeastTemplate(beast));
            node.OnTrigger += HandleTrigger;
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
                MaxMP = template.MagicPoints,
                MaxHP = template.HealthPoints,
                MDefMod = editableBeastTemplate.MagicalDefenseModifier,
                DefMod = template.HasDefenseOverride ? 0 : editableBeastTemplate.DefenseModifier,
                DefOverride = template.HasDefenseOverride ? template.Defense : null,
            };
            // remove old resolved skills
            var oldResolvedSkills = editableBeastModel.Skills.Where(s => s.IsResolved()).ToArray();
            foreach (var oldSkill in oldResolvedSkills)
            {
                editableBeastModel.Skills.Remove(oldSkill);
            }

            var resolverResults = _skillResolver.ResolveSkills(template, input);
            var resolvedSkills = resolverResults.SkillSlots.Where(s => s?.skill != null).Select(s => s.Value.skill).ToArray();           

            foreach(var newResolvedSkills in resolvedSkills)
            {
                editableBeastModel.Skills.Add(newResolvedSkills);
            }

            foreach (var affinityGroup in editableBeastTemplate.Skills.Where(s => s.IsResistanceSkill()).GroupBy(s => s.Id).ToArray())
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void HandleCloseRequested()
	{
		Closing?.Invoke();
	}
}
