using FabulaUltimaNpc;
using FirstProject.Beastiary;
using Godot;

public partial class AddSpellButton : Button
{
    private SpellTemplate _spell;

    [Signal]
    public delegate void AddSpellEventHandler(SignalWrapper<SpellTemplate> equipment);

    public void HandleSpellSelected(SignalWrapper<SpellTemplate> signal)
	{
		_spell = signal.Value;
	}

	public void HandlePressed()
    {
        if(_spell == null) return;
        EmitSignal(SignalName.AddSpell, new SignalWrapper<SpellTemplate>(_spell));
    }

    public void HandleSpellSlotsAvailable(bool spellSlotsEnabled)
    {
        this.Disabled = !spellSlotsEnabled;
    }
}
