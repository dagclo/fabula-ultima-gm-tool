using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;
using System;

public partial class SpellDamageModifierEdit : LineEdit
{
    private SpellTemplate _spell;

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        this.Text = $"{signal.Value.DamageModifier ?? 0}";
        _lastInput = this.Text;
        this.Editable = editable;        
        _spell = signal.Value;
        this.Visible = _spell.IsOffensive;
    }

    private string _lastInput = null;
    public void HandleTextChanged(string newText)
    {
        bool changeSuccess = false;
        if (!string.IsNullOrWhiteSpace(newText) && int.TryParse(newText, out var mpCost))
        {
            _spell.DamageModifier = mpCost;
            changeSuccess = true;
        }
        if (changeSuccess)
        {
            _lastInput = newText;
        }
        else
        {
            this.Text = _lastInput;
        }
    }

    public void HandleToggled(bool on)
    {
        if (on)
        {
            this.Visible = true;
            _spell.DamageModifier = int.Parse(_lastInput);
        }
        else
        {
            this.Visible = false;
            _spell.DamageModifier = null;
        }        
    }
}
