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
        foreach (var child in this.FindChildren("*", recursive: false)
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
        if (editableBeastModel.Species == null) return; // no point in resolving without species
        var input = new SkillInputData
        {
            MaxMP = template.MagicPoints,
            MaxHP = template.HealthPoints,
            MDefMod = template.MagicalDefense,
            DefMod = template.HasDefenseOverride ? 0 : template.Defense,
            DefOverride = template.HasDefenseOverride ? template.Defense : null,
        };
        var skills = _skillResolver.ResolveSkills(template, input);

        editableBeastModel.Skills = skills.SkillSlots.Where(s => s?.skill != null).Select(s => s.Value.skill).ToList();
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
