using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;

public partial class AddedSkillsList : VBoxContainer, IBeastAttribute
{
    private ICollection<SkillTemplate> _skillsList;

    [Export]
    public PackedScene AddSkillScene { get; set; }
    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        _skillsList = beastTemplate.Model.Skills;
    }
}
