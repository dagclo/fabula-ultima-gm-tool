using FirstProject.Encounters;
using Godot;
using System;

public partial class MusicFilePathLineEdit : LineEdit
{
    private Encounter _encounter;

    public void UpdateEncounter(Encounter encounter)
    {
        if (encounter == null) return;
        _encounter = encounter;
        this.Text = _encounter.MusicFilePath;
    }

    public void OnTextChanged(string newText)
    {
        if (_encounter == null) return;
        _encounter.MusicFilePath = newText;
    }
}
