using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class EquipmentList : Label, IBeastAttribute
{
    public Action<System.Collections.Generic.ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Text = string.Join(", ", beastTemplate.Equipment.Select(e  => e.Name));
    }
}
