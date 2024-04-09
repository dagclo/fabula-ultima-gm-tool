using FabulaUltimaDatabase.Models;
using FirstProject.Beastiary;
using Godot;
using System;
public partial class ExpectedDamage : Label
{
	private FirstProject.Npc.Affinity? _affinity;
    private int? _damage;
    private bool _resistanceOverrideActive = false;

    [Signal]
    public delegate void ExpectedDamageChangedEventHandler(int damage);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Text = string.Empty;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnTextChanged(string newText)
    {
		if (!int.TryParse(newText, out int damage)) return; // don't update if not an int	
        _damage = damage;
        SetDamage();
    }

    private void SetDamage()
    {
        if (_damage == null || _affinity == null) return;
        int damageCalculation = Calculate(_damage.Value, _affinity);
        var finalDamage = Math.Abs(damageCalculation);
        this.Text = finalDamage.ToString();
        EmitSignal(SignalName.ExpectedDamageChanged, finalDamage);
    }

    private int Calculate(int damage, FirstProject.Npc.Affinity? affinity)
    {
        double damageMod;
        double resistanceOverride = 1;
        switch (affinity)
        {
            case FirstProject.Npc.Affinity.RESISTANT:                
                damageMod = .5;
                break;
            case FirstProject.Npc.Affinity.IMMUNE:
                damageMod = 0; // does no damage                
                break;
            case FirstProject.Npc.Affinity.ABSORBS:
            case FirstProject.Npc.Affinity.HEAL:
                damageMod = -1; // heals                
                break;
            case FirstProject.Npc.Affinity.VULNERABLE:
                damageMod = 2;
                resistanceOverride = .5;
                break;
            default:
                damageMod = 1;
                resistanceOverride = .5;
                break;
        }

        var overrideFinal = _resistanceOverrideActive ? resistanceOverride : 1;
        return (int) Math.Round(damage * damageMod * overrideFinal, MidpointRounding.ToZero); // rounding down according to pg. 33 of Fabulua Ultima book
    }

    public void OnDamageTypeChanged(SignalWrapper<FirstProject.Npc.Affinity> signal, string _)
	{
		_affinity = signal.Value;
        SetDamage();
    }

    public void OnResistanceOverrideChange(bool toggledOn)
    {
        _resistanceOverrideActive = toggledOn;
        SetDamage();
    }
}
