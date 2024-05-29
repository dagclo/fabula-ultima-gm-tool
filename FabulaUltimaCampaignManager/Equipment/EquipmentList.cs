using FabulaUltimaNpc;
using Godot;
using System;
using System.Linq;

public partial class EquipmentList : Label, IBeastAttribute
{
    public Action<bool> Save { get; set; }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        this.Text = string.Join(", ", beastTemplate.Equipment.Select(e  => e.Name));
    }
}
