using FirstProject.Encounters;
using Godot;
using System;

public partial class VictoryMusicLineEdit : LineEdit
{
    private Encounter _encounter;

    public void UpdateEncounter(Encounter encounter)
    {
        if (encounter == null) return;
        _encounter = encounter;
        this.Text = _encounter.VictoryMusicFilePath;
    }

    public void OnTextChanged(string newText)
    {
        if (_encounter == null) return;
        _encounter.VictoryMusicFilePath = newText;
        if (this.Text != newText) this.Text = newText;
    }
}
