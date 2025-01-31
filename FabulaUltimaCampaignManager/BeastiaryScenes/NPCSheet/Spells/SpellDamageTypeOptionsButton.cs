using FabulaUltimaNpc;
using FabulaUltimaSkillLibrary;
using FirstProject.Beastiary;
using Godot;
using System.Linq;

public partial class SpellDamageTypeOptionsButton : OptionButton
{
    private SpellTemplate _spellTemplate;
    private FabulaUltimaNpc.DamageType _current = null;

    [Export]
    public Godot.Collections.Array<string> DamageTypes { get; set; } = new Godot.Collections.Array<string>(DamageConstants.DamageTypes);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach ((var val, int index) in DamageTypes.Select((a, i) => (a, i)))
        {
            AddItem(val, index);
        }
        Select(-1);
    }

    public void OnSelected(int index)
    {
        if (_spellTemplate == null) return;
        var damageTypeName = DamageTypes[GetItemId(index)];
        _spellTemplate.DamageType = new FabulaUltimaNpc.DamageType
        {
            Id = DamageConstants.DamageTypeMap[damageTypeName],
            Name = damageTypeName,
        };
        _current = _spellTemplate.DamageType;
    }

    public void HandleSpellSet(SignalWrapper<SpellTemplate> signal, bool editable)
    {
        var spell = signal.Value;
        _spellTemplate = spell;
        this.Disabled = !editable;
        this.Visible = _spellTemplate.IsOffensive;
        if (_spellTemplate.DamageType == null) return;
        var id = DamageTypes.IndexOf(_spellTemplate.DamageType.Name.ToLowerInvariant());
        Select(GetItemIndex(id));
        _current = _spellTemplate.DamageType;
    }

    public void HandleToggled(bool on)
    {
        if (on)
        {
            this.Visible = true;
            _spellTemplate.DamageType = _current;
        }
        else
        {
            this.Visible = false;
            _spellTemplate.DamageType = null;
        }
    }
}
