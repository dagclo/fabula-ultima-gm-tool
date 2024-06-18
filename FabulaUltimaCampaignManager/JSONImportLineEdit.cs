using FabulaUltimaGMTool.Beastiary.Import.Fultimator;
using FirstProject;
using FirstProject.Beastiary;
using FirstProject.Messaging;
using Godot;
using System;

public partial class JSONImportLineEdit : TextEdit
{
    [Signal]
    public delegate void JsonErrorEventHandler(string error);

    [Signal]
    public delegate void BeastTemplateAddedEventHandler();

    private BeastiaryRepository _repository;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _repository = GetNode<DbAccess>("/root/DbAccess").Repository;
    }

    public void HandleImportButtonClicked()
    {
        if(FultimatorHelper.TryParseJson(this.Text, out var result))
        {
            _repository.AddBeastTemplate(result.AsBeastTemplate());
            EmitSignal(SignalName.BeastTemplateAdded);
        }
        else
        {
            EmitSignal(SignalName.JsonError, "invalid json");
        }
    }
}
