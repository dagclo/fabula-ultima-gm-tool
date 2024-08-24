using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class MpCostEdit : LineEdit
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        this.Text = $"{signal.Value.MagicPointCost}";
        _lastInput = this.Text;
        this.Editable = editable;
        _spell = signal.Value;
    }

    private string _lastInput = null;
    public void HandleTextChanged(string newText)
    {
        bool changeSuccess = false;
        if (!string.IsNullOrWhiteSpace(newText) && int.TryParse(newText, out var mpCost))
        {
            _spell.MagicPointCost = mpCost;
            changeSuccess = true;
        }
        if(changeSuccess)
        {
            _lastInput = newText;
        }
        else
        {
            this.Text = _lastInput;
        }        
    }
}
