using FabulaUltimaGMTool.BeastiaryScenes;
using FabulaUltimaNpc;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ValidationsList : VBoxContainer, IBeastAttribute
{
    private NpcSheet _npcSheet;

    [Export]
    public string StartNodePath { get; set; }

    [Signal]
    public delegate void IsBeastValidEventHandler(int errorCount, int warningCount);

    public Action<ISet<BeastEntryNode.Action>> BeastTemplateAction { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _npcSheet = GetNode(StartNodePath) as NpcSheet;        
    }

    public void HandleBeastChanged(IBeastTemplate beastTemplate)
    {
        // clear prior validations
        var children = this.FindChildren("*", recursive: false);
        foreach (var child in children)
        {
            RemoveChild(child);
            child.QueueFree();
        }

        int errors = 0;
        int warnings = 0;
        foreach(var validatable in _npcSheet.FindChildren("*", owned: false).Where(c => c is IValidatable).Select(c => c as IValidatable))
        {
            foreach(var validation in validatable.Validate())
            {
                var validationLabel = new Label
                {
                    Text = $"{validation.Level}: {validatable.Name} - {validation.Message}"
                };
                AddChild(validationLabel);
                validationLabel.Owner = this;
                if (validation.Level == ValidationLevel.ERROR) errors++;
                if (validation.Level == ValidationLevel.WARNING) warnings++;
            }           
        }
        EmitSignal(SignalName.IsBeastValid, errors, warnings);
    }
}
