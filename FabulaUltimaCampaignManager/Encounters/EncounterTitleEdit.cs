using FirstProject.Encounters;
using Godot;

public partial class EncounterTitleEdit : LineEdit
{
    private Encounter _encounter;

    public void UpdateEncounter(Encounter encounter)
    {
        if (encounter == null) return;
        _encounter = encounter;       
        this.Text = _encounter.Name;
    }

    public void OnTextChanged(string newText)
    {
       if(_encounter == null) return;        
       _encounter.Name = newText;
    }
}
